using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.UI.Drawing
{
    public class FontCache : IDisposable
    {
        #region VARIABLES
        private Dictionary<int, Font> fonts;
        private Renderer renderer;
        #endregion

        #region PROPERTIES
        #endregion

        #region CONSTRUCTORS
        internal FontCache(Renderer renderer)
        {
            fonts = new Dictionary<int, Font>();
            this.renderer = renderer;
        }
        #endregion

        #region METHODS
        public Font this[Font font]
        {
            get
            {
                //We haven't cached null, neither can we init fonts with an invalid renderer
                if (font == null || !renderer.Initialized)
                    return font;

                //Check cached fonts
                if (fonts.ContainsKey(font.ID))
                {
                    var fnt = fonts[font.ID];
                    if (fnt == null) //This should NEVER happen
                        throw new InvalidOperationException("Font can not be null!");

                    //Font isn't disposed so, return it
                    if (!fnt.IsDisposed)
                        return fnt;

                    //Remove disposed fonts
                    fonts.Remove(font.ID);
                    //Create a new one
                    fnt = renderer.CreateFont(fnt);
                    //Discard it if creation failed though
                    if (fnt != null && !fnt.IsDisposed)
                        return fonts[font.ID] = fnt;
                }
                else //Cache font and check it
                {
                    fonts[font.ID] = font;
                    return this[font];
                }
                //Something went wrong; Return null
                return null;
            }
        }

        internal Font GetFont(string family, float height)
        {
            //Get cached font
            if (fonts.Values.Any(x => x != null && !x.IsDisposed && x.Family == family && x.Height == height))
                return fonts.Values.First(x => x != null && !x.IsDisposed && x.Family == family && x.Height == height);

            //Return null if we don't have any
            return null;
        }

        public void Dispose()
        {
            foreach (var fnt in fonts.Values)
                if (fnt != null && !fnt.IsDisposed)
                    fnt?.Dispose();
            fonts.Clear();
        }
        #endregion
    }
}
