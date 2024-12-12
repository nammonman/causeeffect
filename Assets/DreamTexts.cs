using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamTexts : MonoBehaviour
{
    public static List<Dream> dreams;

    void Start()
    {
        // Initialize the List
        dreams = new List<Dream>();

        // Add dreams 

        // dream 1
        dreams.Add(new Dream
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
                }
            }
        });

        // dream 2
        dreams.Add(new Dream
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
                }
            }
        });

        // dream 2
        dreams.Add(new Dream
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
        });

    }

}