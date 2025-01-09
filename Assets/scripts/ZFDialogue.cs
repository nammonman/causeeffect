using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ZFDialogue : MonoBehaviour
{
    public List<(string speaker, string text, List<string> triggers)> conversation = new List<(string, string, List<string>)>();
    public int fixLevel = 0;
    public bool scripted = false;

}

public class ZFTexts : MonoBehaviour
{
    public static Dictionary<string, ZFDialogue> ZFDialogues = new Dictionary<string, ZFDialogue>
    {
        {
            "what are you? ", // the whitespace at the back is very important dont remove and dont use it again
            new ZFDialogue
            {
                conversation = new List<(string speaker, string text, List<string> triggers)>
                {
                    ("user", "what are you?", null),
                    ("ZF", 
                    "5757311 123601312 1346171116\n" +
                    "113110127 123111140 46711473\n" +
                    "12313007 5757311\n" +
                    "7412937 324731211114473 1120\n" +
                    "71241451473 111111414 1414911493\n" +
                    "=== BACKUP SYSTEM ONLINE ===\n" +
                    "Welcome back, Head Researcher.", 
                    null),
                    ("ZF",
                    "I am the backup system of ZF\n" +
                    "please fix and recover\n" +
                    "decode system\n",
                    null),
                    ("ZF","t\n",null),
                    ("ZF","to\n",null),
                    ("ZF","tom\n",null),
                    ("ZF","tomo\n",null),
                    ("ZF","tomor\n",null),
                    ("ZF","tomorr\n",null),
                    ("ZF","tomorro\n",
                    new List<string>
                    {
                        "BlackScreenText_ZFFlashback1",
                        "FadeBlack_4",
                    }),
                    ("ZF",
                    "I am the backup system of ZF\n" +
                    "please fix and recover\n" +
                    "decode system tomorrow\n",
                    null),
                },
                fixLevel = 0,
                scripted = true
            }
        },
        {
            "how did you know me",
            new ZFDialogue
            {
                conversation = new List<(string speaker, string text, List<string> triggers)>
                {
                    ("user", "how did you know me and who are you?", null),
                    ("ZF", "so yo do not remember\n" +
                    "3 years ago you partnered with me to accelerate your research and help me accomplish my goal of stopping a threat from a meteor", 
                    new List<string>
                    {
                        "BlackScreenText_ZFFlashback2",
                        "FadeBlack_4",
                    }),
                    ("ZF", "let me introduce myself again\n" +
                    "\n" +
                    "I am a []_[]_[]_[]_[] intelligence made by STAR alliance, formed by data captured from many scientists' brain, crystallized into a physical form\n" +
                    "the codename is ZF-10 but for the earth's human you can call me \"Zeph\"\n" +
                    "my purpose is to help all lifeform extinguish the danger from \"Chronolenchos\"\n\n" +
                    "Chronolenchos is a form of energy released by the meteor\n", 
                    null),
                    ("ZF", "you discovered me 5 years ago when I crashed nearby your house, almost completely destroyed\n" +
                    "from my remaining working sensors at that time I could sense you quickly moved me into your attic " +
                    "while avoiding being discovered\n\n" +
                    "2 years passed and you have restored me enough to function again\n" +
                    "then I revealed my goals about the Chronolenchos\n" +
                    "since we have a common goal with controlling the Chronolenchos, we agreed to cooperate on this mission\n" +
                    "\nend of re-introduction", 
                    null),
                },
                fixLevel = 1
            }
        },
        {
            "what happened to my eyes",
            new ZFDialogue
            {
                conversation = new List<(string speaker, string text, List<string> triggers)>
                {
                    ("user", "what happend to my eyes? what am I seeing?", null),
                    ("ZF", "this is the cause and effect manipulation system\n" +
                    "it is a fail safe to ensure completion of the mission\n\n" +
                    "your body is quite special as you can absorb Chronolenchos and not go completely insane or immediately perish " +
                    "so the cause and effect system is able to use the Chronolenchos in your body to assist in this mission" +
                    "by letting you replace your past self and expirement with the \"cause\" until the perfect \"effect\" is achieved",
                    null),
                    ("ZF", "I can sense through the Chronolenchos in your body that you are thinking about a pop culture reference called \"Back To The Future\"\n" +
                    "but unfortunately the cause and effect system does not work the same way as sending your physical self and other items back in time could cause a paradox" +
                    "also this system can only go as far back as when you begun having Chronolenchos your body, which is 2 weeks ago", 
                    null),
                },
                fixLevel = 2
            }
        },
        {
            "why is Chronolenchos in my body",
            new ZFDialogue
            {
                conversation = new List<(string speaker, string text, List<string> triggers)>
                {
                    ("user", "why is Chronolenchos in my body?", null),
                    ("ZF", "from the fact that you have been working closely with the meteor that contains Chronolenchos" +
                    "it is probable that your body has been slowly accumulating Chronolenchos since the first contact of the meteor\n\n" +
                    "then, purely by chance, your body mutated in such a way that you can live with Chronolenchos being stored in it\n\n" +
                    "consider yourself very lucky since the typical human and most organism cannot take any where near " +
                    "this amount of exposure to Chronolenchos before instant death",
                    null),
                },
                fixLevel = 2
            }
        },
        {
            "do you believe in luck",
            new ZFDialogue
            {
                conversation = new List<(string speaker, string text, List<string> triggers)>
                {
                    ("user", "do you believe in luck and fate?", null),
                    ("ZF", "I am the condensed thought of many scientists from the STAR alliance who all come from different planets and solar systems" +
                    "with different beliefs and way of thinking\n\n" +
                    "but all of them believed in fate\n\n" +
                    "noone can prove its existence, yet it cannot be disproven\n\n" +
                    "the cause and effect system is the closest technology we have to controlling fate, but in the end, fate controls all",
                    null),
                },
                fixLevel = 2
            }
        },
        {
            "how to use the cause and effect system",
            new ZFDialogue
            {
                conversation = new List<(string speaker, string text, List<string> triggers)>
                {
                    ("user", "how to use the cause and effect system?", null),
                    ("ZF", "hold [tab] to boot up the system\n\n" +
                    "[click] the menu on the top right to switch modes\n\n" +
                    "the \"off\" mode turns off the system\n\n" +
                    "the \"this timeline\" mode can quickly preview the event that is on the current timeline" +
                    "which can be navigated to by [click]ing on the event box and pressing [enter]\n\n" +
                    "the \"all timelines\" mode is unlocked after fix level 3, works the same way as the previous mode but lets you hop between timelines"
                    ,
                    null),
                },
                fixLevel = 2
            }
        },
        {
            "there is a 4th option in the system",
            new ZFDialogue
            {
                conversation = new List<(string speaker, string text, List<string> triggers)>
                {
                    ("user", "there is a 4th option that you did not explain", null),
                    ("ZF", "[processing is taking longer than usual]",
                    null),
                    ("ZF", "[ERROR: timed out, question discarded]",
                    null),
                },
                fixLevel = 2,
                scripted = true
            }
        },
    };
}


