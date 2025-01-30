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
            dreamName = "Zeph takeover",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "Aaagh.....Aaargh!!!",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: synchronization level: 38%",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "you betrayed me...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: synchronization level: 54%",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "I should have known...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: synchronization level: 79%",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: you should be proud that you are our first step to...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "no...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: synchronization level: 100%",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },

        // dream 2
        new Dream
        {
            dreamName = "exposed",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "a gunshot is fired. you have been shot.",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: you could have avoided this if you accepted my offer",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "that thing will destroy humanity! why are you doing this?",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: I know we can control it.",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: do you really think that I can't just get someone better than you to continue the project",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: you are not the main character, don't be stupid now.",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },

        // dream 3
        new Dream
        {
            dreamName = "war 1",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "A: no!!! its over for humanity!!!",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "B: not my son...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "I could have stopped it. why...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "let me redo this loop again... please...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "I know it will work...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },

        new Dream
        {
            dreamName = "war 2",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "no...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: breaking news. many nuclear weapons have been detonated by country...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "its still not working...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "try... again...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },

            }
        },
        new Dream
        {
            dreamName = "war 6",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "the war happened six times now...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "those \"dreams\" really did happen in another timeline...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "ZF: that is a possibility... before... failure...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "ZF: but the density of Chronolencos... breaks the system... return...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "I hope this can reach me, in another loop, in another timeline...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "hello future me... failed the mission... the war rages on... it has been activated...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "The President is not tie only traitor... could be one of the assistants...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "...deal with them next time",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },

        new Dream
        {
            dreamName = "war ???",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "...countless variations of this war has happened...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "...should I just give up?...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: I was chosen by the Chronolenchos as the initializer. now I must challenge and evolve humans to become the ideal race",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: the Chronolenchos chose humans for a reason. now its your turn to evolve with us",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "you're A?...how...you were dead...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "A: my death was greatly exaggerated. I am now one with the Chronolenchos",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },
        new Dream
        {
            dreamName = "zeph achieved",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "what is that? are you kidding me!",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "???: ...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "answer me!",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "ZF: sorry, head researcher, but it is mmy duty to stop humanity from using Chronolenchos",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "ZF: but don't fear. humanity will be safe under our government",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },
        new Dream
        {
            dreamName = "????",
            BGSoundFileName = ".mp3",
            dreamTexts = new List<DreamText>
            {
                new DreamText
                {
                    text = "finally...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "...I've done it...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "...revert to...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "...the cycle will end...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "...I have to note it down...now...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
                new DreamText
                {
                    text = "...before it's too late...",
                    playSoundFileName = ".mp3",
                    playEffectName = ""
                },
            }
        },
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
                        text = "<b>INITIATE CAUSEEFFECT PROTOCOL</b>\nTarget: 1 week ago",
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
            "tutorial",
            new Dream
            {
                dreamName = "tutorial",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "A: this is the substance mixer",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "A: you can use the interface to create a mixture with certain properties",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "A: simply drag a block from the right side to the middle grid to add it, and drag a block out to remove it",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "A: you can also press [R] to rotate the blocks if needed",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "A: last but not least, just keep an eye on the 4 numbers on the top right and make sure it isn't in the dangerous list",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "A: try creating a mixture.",
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
        {
            "ending 1",
            new Dream
            {
                dreamName = "ending 1",
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
                        text = "its time!",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "???: we are going to temporarily interrupt this presentation",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "???: Head Researcher, come with us. this is urgent",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
        {
            "ending 1 finale",
            new Dream
            {
                dreamName = "ending 1 finale",
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
                        text = "<b>INITIATE CAUSEEFFECT PROTOCOL</b>\nTarget: 1 week ago",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
        {
            "ending 2",
            new Dream
            {
                dreamName = "ending 2",
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
                        text = "its time!",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"I also have something else to tell you\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"if you look at your phones right now there is a file that just opened\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"please take a look at it\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"the true intentions of the government, who will be taking over this research project\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "\"see it with your own eyes and-\"",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "before you can say the whole speech, everything turned bright.",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "light engulfs the entire room",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                }
            }
        },
        {
            "ending 2 finale",
            new Dream
            {
                dreamName = "ending 2 finale",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "your duty is now over. I will take over from here",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "synchronization level: 38%\nthank you for being an important step in our plan",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "synchronization level: 79%\nplease be proud. assets as good as you are hard to come by",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "synchronization level: 100%\ncontinue with the next mission.",
                        playSoundFileName = "",
                        playEffectName = ""
                    },
                    new DreamText
                    {
                        text = "<b>MISSION COMPLETE</b>\nCause of death: [][][][][][][] [][][][][][][]",
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
            "ending 3 finale",
            new Dream
            {
                dreamName = "ending 3 finale",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "<b>MISSION FAILED</b>\nCause of death: [][][][][][][] [][][][][][][]",
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
            "dangerous mix",
            new Dream
            {
                dreamName = "dangerous mix",
                BGSoundFileName = "",
                dreamTexts = new List<DreamText>
                {
                    new DreamText
                    {
                        text = "<b>MISSION FAILED</b>\nCause of death: sudden exposure to dangerous substance",
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
    };

}