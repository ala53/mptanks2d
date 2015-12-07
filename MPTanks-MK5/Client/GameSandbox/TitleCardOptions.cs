using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox
{
    public static class TitleCard
    {

        public static readonly string[] Options = {
                "I hate pancakes",
                "We're not taking over the world, we swear...",
                "HOLY SHIT CATS",
                "You this read wrong",
                "It's not a bug, it's a feature",
                "Don't hit the big red button",
                "No hax, just l33t skillz", //David...just...David
                "\"Gimme a minute to think\"", //Casey when asked about this
                "Definitely not spyware", //Morgan
                "Always watching you, but in a not-creepy, \"friendly\" stalker way",
                "Establishing connection to Skynet", //Casey
                "Now featuring: the main menu!", //Morgan
                "<?php ERROR: python.lang::ArithmeticException",
                "Code causes retinal spasms in 3/5 leading programmers",
                "It's not murder, it's science!", //Xander
                "I just wanna know if penguins have knees", //Xander
                "Bug report: Game not found" //Jacob
        };

        public static string Option() => Options[new Random().Next(0, Options.Length)];
    }
}
