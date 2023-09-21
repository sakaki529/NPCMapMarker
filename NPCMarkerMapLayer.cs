using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace NPCMapMarker
{
    public class NPCMarkerMapLayer : ModMapLayer
    {
        private Asset<Texture2D> marker;

        private const byte MaxFrame = 4;
        private const byte FrameRate = 8;
        private byte frame;
        private byte frameCounter;

        public override void Load()
        {
            if (!Main.dedServ)
                marker = Request<Texture2D>(NPCMapMarker.AssetsTexturePath + "NPCMarker", AssetRequestMode.ImmediateLoad);
        }

        public override void Unload()
        {
            if (!Main.dedServ)
                marker = null;
        }

        private void Animate()
        {
            if (++frameCounter >= FrameRate)
            {
                frameCounter = 0;
                if (++frame >= MaxFrame)
                    frame = 0;
            }
        }

        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            Animate();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.GetBossHeadTextureIndex() == -1 && NPC.TypeToDefaultHeadIndex(npc.type) <= 0)
                {
                    Color color = Color.MediumVioletRed;
                    if (npc.CountsAsACritter || npc.friendly)
                        color = Color.RoyalBlue;
                    color.A = 200;
                    context.Draw(marker.Value, npc.Center / 16f, color, new SpriteFrame(1, 4, 0, frame), 1.5f, 1.5f, Alignment.Center);
                }
            }
        }
    }
}
