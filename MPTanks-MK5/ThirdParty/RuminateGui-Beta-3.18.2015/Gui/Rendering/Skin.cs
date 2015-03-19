using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ruminate.GUI.Framework {

    public class Skin {

        internal Texture2D ImageMap { get; private set; }

        public Dictionary<string, Renderer> WidgetMap { internal get; set; }        

        public Skin(Texture2D imageMap, string map) {

            ImageMap = imageMap;

            WidgetMap = new Dictionary<string, Renderer>();

            map = map.Replace("\r", String.Empty);
            foreach (var line in map.Split('\n')) {

                if (string.IsNullOrWhiteSpace(line)) { return; }

                var equalsSplit = line.Split('=');
                var optionSplit = equalsSplit[1].Split(',');

                var renererType = typeof(Renderer);
                var rendererTypes = new List<Type>();
                foreach (var assemply in AppDomain.CurrentDomain.GetAssemblies()) {
                    rendererTypes.AddRange(assemply.GetTypes().Where(t => t != renererType && renererType.IsAssignableFrom(t)));
                }

                foreach (var renderer in rendererTypes) {

                    if (renderer.Name != (optionSplit[1]).Trim()) { continue; }

                    var rectangleSplit = (optionSplit[0]).Trim().Split(' ');
                    var area = new Rectangle(
                        Int32.Parse(rectangleSplit[0]),
                        Int32.Parse(rectangleSplit[1]),
                        Int32.Parse(rectangleSplit[2]),
                        Int32.Parse(rectangleSplit[3]));

                    var argTypes = new List<Type> { typeof(Texture2D), typeof(Rectangle) };
                    for (var x = 2; x < optionSplit.Length; x++) {
                        argTypes.Add(typeof(int));
                    }

                    var args = new List<Object> { imageMap, area };
                    for (var x = 2; x < optionSplit.Length; x++) {
                        args.Add(int.Parse(optionSplit[x]));
                    }

                    var c = renderer.GetConstructor(argTypes.ToArray());
                    if (c != null) {
                        WidgetMap.Add((equalsSplit[0]).Trim(), (Renderer)c.Invoke(args.ToArray()));
                    }
                }
            }
        }
    }
}
