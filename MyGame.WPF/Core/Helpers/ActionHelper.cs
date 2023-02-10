using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;
using MVVMEssentials.Services;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;
using MyGame.WPF.MVVM.Models.Actions;
using MyGame.WPF.MVVM.Models.Npcs;
using Newtonsoft.Json;

namespace MyGame.WPF.Core.Helpers;

public static class ActionHelper {
    public static async Task HandleGreeting(SaveStore saveStore, StringStore stringStore, INavigationService informationNavigationService) {
        Save save = saveStore.CurrentSave!;
        Npc npc = save.NpcAction!;

        var textline = new Textline();

        save.SerializableTextLines.Clear();

        try {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MyGame.WPF.Resources.JSON.Greetings.Npc.json") ??
                            throw new InvalidOperationException("The file Greetings/Npc.json doesn't exist.");

            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();

            List<Greeting> greetings = JsonConvert.DeserializeObject<List<Greeting>>(result)!;

            string? filePathCustom = npc switch {
                CustomNpc => $"MyGame.WPF.Resources.JSON.Greetings.{nameof(CustomNpc)}.json",
                _ => null
            };

            if (filePathCustom is not null) {
                stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filePathCustom) ??
                         throw new InvalidOperationException($"Couldn't find {filePathCustom}.");

                reader = new StreamReader(stream);
                result = reader.ReadToEnd();

                List<Greeting> greetingsCustom = JsonConvert.DeserializeObject<List<Greeting>>(result)!;

                foreach (Greeting g in greetings) {
                    foreach (Greeting gCustom in greetingsCustom) {
                        if (gCustom.Id == g.Id) {
                            if (gCustom.Text is not null) {
                                g.Text = gCustom.Text;
                            }

                            if (gCustom.MinRelationship is not null) {
                                g.MinRelationship = gCustom.MinRelationship;
                            }

                            if (gCustom.MaxRelationship is not null) {
                                g.MaxRelationship = gCustom.MaxRelationship;
                            }
                        }

                        if (greetings.All(x => x.Id != gCustom.Id)) {
                            greetings.Add(gCustom);
                        }
                    }
                }
            }

            Greeting? greeting = null;
            foreach (Greeting g in greetings) {
                if (npc.Relationship >= g.MinRelationship && npc.Relationship < g.MaxRelationship) {
                    greeting = g;
                }
            }

            if (greeting == null) {
                throw new InvalidOperationException("No greeting found.");
            }

            textline.TextParts.Add(new Tuple<Color, string>(npc.Color, greeting.Text!));
            save.AddSerializableTextLines(textline);

            saveStore.Refresh();

            await HandleTalk(saveStore, stringStore, informationNavigationService, null);
        } catch (Exception e) {
            stringStore.CurrentString = e.Message;
            informationNavigationService.Navigate();
        }
    }

    public static async Task HandleTalk(SaveStore saveStore, StringStore stringStore, INavigationService informationNavigationService, TalkAction? action) {
        Save save = saveStore.CurrentSave!;
        Npc npc = save.NpcAction!;
        bool endConversation = false;

        save.PlayerCanAct = false;

        var textline = new Textline();

        TalkAction? talkAction = null;
        TalkActionResult? talkActionResult = null;

        try {
            if (action is not null) {
                List<TalkAction> talkActions = GetTalkActions(save, npc);

                talkAction = talkActions.FirstOrDefault(ta => ta.Id == action.Id)!;

                if (talkAction.EndConversation == true) {
                    endConversation = true;
                }

                if (talkAction.PlayerDialog is not null) {
                    textline = new();
                    textline.TextParts.Add(new Tuple<Color, string>(Colors.Honeydew, talkAction.PlayerDialog));
                    save.AddSerializableTextLines(textline);
                    saveStore.Refresh();
                    await Task.Delay(500);
                }


                foreach (TalkActionResult taR in talkAction.Results!) {
                    if (npc.Relationship >= taR.MinRelationship && npc.Relationship < taR.MaxRelationship) {
                        talkActionResult = taR;
                    }
                }

                if (talkActionResult!.EffectRelationship is not null) {
                    npc.Relationship += talkActionResult.EffectRelationship ?? default(int);
                }
                
                if (talkActionResult.AddedMinutes is not null) {
                    save.World.Date = save.World.Date.AddMinutes(talkActionResult.AddedMinutes ?? default(int));
                }

                if (talkActionResult.NpcDialog is not null) {
                    textline = new();
                    textline.TextParts.Add(new Tuple<Color, string>(npc.Color, talkActionResult.NpcDialog));
                    save.AddSerializableTextLines(textline);
                    saveStore.Refresh();
                    await Task.Delay(500);
                }
            }
            
            if (npc.GetLocation(save.World.Date) == null || !npc.GetLocation(save.World.Date)!.Item1.Equals(save.Situation.LocationName)) {
                endConversation = true;
                
                textline = new();
                textline.TextParts.Add(new Tuple<Color, string>(npc.Color, "Sorry, I have to go."));
                save.AddSerializableTextLines(textline);
                saveStore.Refresh();
                await Task.Delay(500);
            }

            save.AddBlankTextline();

            if (!endConversation) {
                if (talkAction == null || talkActionResult == null || talkActionResult.Success == null || talkActionResult.Success == false) {
                    SetPossibleTalkActions(save, npc);
                } else if (talkAction.NextTalkActions.Count > 0) {
                    SetNextTalkActions(save, npc, talkAction.NextTalkActions);
                }
            } else {
                save.PossibleTalkActions.Clear();
                save.IsInChat = false;
                save.NpcAction = null;
            }

            save.PlayerCanAct = true;
            saveStore.Refresh();
        } catch (Exception e) {
            stringStore.CurrentString = e.Message;
            informationNavigationService.Navigate();
        }
    }

    private static void SetNextTalkActions(Save save, Npc npc, List<NextTalkAction> nextTalkActions) {
        save.PossibleTalkActions.Clear();

        List<TalkAction> talkActions = GetTalkActions(save, npc);

        foreach (TalkAction ta in talkActions) {
            foreach (NextTalkAction nTa in nextTalkActions) {
                if (ta.Id == nTa.NextActionId && ta.NeedPrevious == true) {
                    save.PossibleTalkActions.Add(ta);
                }
            }
        }
    }

    public static void SetPossibleTalkActions(Save save, Npc npc) {
        save.PossibleTalkActions.Clear();

        List<TalkAction> talkActions = GetTalkActions(save, npc);

        foreach (TalkAction ta in talkActions) {
            if (ta.NeedPrevious is null) {
                save.PossibleTalkActions.Add(ta);
            }
        }
    }

    public static List<TalkAction> GetTalkActions(Save save, Npc npc) {
        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MyGame.WPF.Resources.JSON.TalkActions.Npc.json") ??
                        throw new InvalidOperationException("The file TalkActions/Npc.json doesn't exist.");

        StreamReader reader = new StreamReader(stream);
        string result = reader.ReadToEnd();

        List<TalkAction> talkActions = JsonConvert.DeserializeObject<List<TalkAction>>(result)!;

        string? filePathCustom = npc switch {
            CustomNpc => $"MyGame.WPF.Resources.JSON.TalkActions.{nameof(CustomNpc)}.json",
            _ => null
        };

        if (filePathCustom is not null) {
            stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filePathCustom) ??
                     throw new InvalidOperationException($"Couldn't find {filePathCustom}.");

            reader = new StreamReader(stream);
            result = reader.ReadToEnd();

            List<TalkAction> talkActionsCustom = JsonConvert.DeserializeObject<List<TalkAction>>(result)!;

            foreach (TalkAction ta in talkActions) {
                foreach (TalkAction gTa in talkActionsCustom) {
                    if (gTa.Id == ta.Id) {
                        if (gTa.Label is not null) {
                            ta.Label = gTa.Label;
                        }

                        if (gTa.PlayerDialog is not null) {
                            ta.PlayerDialog = gTa.PlayerDialog;
                        }

                        if (gTa.Results is not null) {
                            ta.Results = gTa.Results;
                        }
                    }

                    if (talkActions.All(x => x.Id != gTa.Id)) {
                        talkActions.Add(gTa);
                    }
                }
            }
        }

        return talkActions;
    }
}