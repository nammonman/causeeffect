using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamTexts : MonoBehaviour
{
    public static List<Dream> dreams = new List<Dream>
    {
        // Add dreams 
        
        // dream 0
        new Dream
        {
            dreamName = "no dream",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "Tonight is a peaceful night",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },
        // dream 1
        new Dream
        {
            dreamName = "Peaceful Garden",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "You find yourself in a serene garden...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "The sun warms your skin...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "ChatGPT totally did not write this",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "lol",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },

        // dream 2
        new Dream
        {
            dreamName = "Mystic Forest",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "The forest is dark but full of life...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "You hear the rustling of leaves...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "hi",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },

            }
        },

        // dream 3
        new Dream
        {
            dreamName = "Starry Night",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "The stars shine brightly above you...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "A shooting star crosses the sky...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                }
            }
        }
        

    };

    public static Dictionary<string, Dream> blackScreenTexts = new Dictionary<string, Dream>
    {
        // Add dreams 
        
        // blackScreenTexts 0

        {
            "default",
            new Dream
            {
                dreamName = "Echoing Caves",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "The sound of dripping water echoed endlessly.",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "Shadows flickered on the damp cave walls.",
                        playSoundFileName = "",
                        playEffectName = ""
                    }
                }
            }
        },
        {
            "firstSequence",
            new Dream
            {
                dreamName = "firstSequence",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "The screen behind you changed to show the live camera feed from the lab",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "It is showing the result of you and your team's hard work",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"The machine that could solve earth's impending energy crisis\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "Noises and a very bright light starts emitting from the machine",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "There it is! It is working just as planned",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "Applause fills the conference room",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "Everything is going very well! you thought",
                        playSoundFileName = "",
                        playEffectName = ""

                    },
                    new DreamText
                    {
                        text = "Too well..",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "A loud crack can be heard from the machine, video feed cuts and the room went silent",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "You have to explain this, head researcher...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
        {
            "presidentShootPlayer",
            new Dream
            {
                dreamName = "presidentShootPlayer",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "<b>MISSION FAILED</b>\nCause of death: Shot with projectile weapon",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "<b>INITIATE CAUSEEFFECT PROTOCOL</b>\nTarget: 2 weeks ago",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
        {
            "jokeEnding",
            new Dream
            {
                dreamName = "jokeEnding",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "<b>MISSION FAILED</b>\nCause of death: Abondoned mission",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "<b>INITIATE CAUSEEFFECT PROTOCOL</b>\nTarget: 1 week ago",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
        {
            "goToDoctor",
            new Dream
            {
                dreamName = "goToDoctor",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "You go to the appointment.",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"you have been diagnosed with head trauma so that is likely the cause of memory loss\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"do not worry though, as this is not life threathening and the memory loss is not permanent\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"just take it easy and your memory will slowly come back\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "The appointment took until the evening, so you go home to prepare for the next day.",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "But as you go upstairs, you see something unexpected.",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"what are you???\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
        {
            "ZFFlashback1",
            new Dream
            {
                dreamName = "ZFFlashback1",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "you suddenly remember something...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "you see a destroyed machine",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "maybe its a fallen spacecraft or satellite?",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"my codename is _____\" ",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "doesn't quite roll off the tongue, how about I call you \"Zeph\" instead?",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"do you agree to work with me?\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "for humanity, I agree!",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "even if I have to use alien technology...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"I have never seen such lifeform that can handle this much...\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
        {
            "ZFFlashback2",
            new Dream
            {
                dreamName = "ZFFlashback2",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "you suddenly remember something...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "are you an alien? a spy? or a weapon?",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"[][][][][] I am [][][][] codename ZF-10 from [][][][][][][] 57412 [][] 411141463 [][][][]\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"[][][] the mission [][][][][] stop the [][][] 611120140131461105 [][][]\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"the meteor [][][][] energy research [][][][][][] 037367 [][] prevent humans [][][][][]\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "so this thing knows about the meteor...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "...",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"thank you very much, Head Researcher\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"in exchange for repairing me, I will do my best to assist you in your research\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "finally! I can communicate with this thing",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"I hope we can assist each other in accomplishing our missions\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
    };

}