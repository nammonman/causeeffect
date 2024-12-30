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

    };

}