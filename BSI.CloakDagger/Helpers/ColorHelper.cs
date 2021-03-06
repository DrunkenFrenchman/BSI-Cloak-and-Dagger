﻿using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace BSI.CloakDagger.Helpers
{
    public static class ColorHelper
    {
        public static Color GetRandomColor()
        {
            return Color.FromUint(Colors.All[new Random(Environment.TickCount).Next(Colors.All.Count)]);
        }

        public static class Colors
        {
            public static readonly List<uint> All = new List<uint>
            {
                0xffB57A1E,
                0xff4E1A13,
                0xff284E19,
                0xffB4F0F1,
                0xff793191,
                0xffFCDE90,
                0xff382188,
                0xffDEA940,
                0xff591645,
                0xffFFAD54,
                0xff429081,
                0xffEFC990,
                0xff224277,
                0xffCEDAE7,
                0xff8D291A,
                0xffF7BF46,
                0xff6bd5dc,
                0xffeed690,
                0xffaec382,
                0xffc3c3c3,
                0xffd5d7d4,
                0xffe7ecd6,
                0xffeaeeef,
                0xff7f6b60,
                0xff967e7e,
                0xffb6aba7,
                0xffe7d3ba,
                0xffeae1da,
                0xffd9dbce,
                0xffdfd6cd,
                0xffcac1ba,
                0xffece8dd,
                0xffe0dcd9,
                0xffefece5,
                0xffeae9e5,
                0xfff5f5f5,
                0xfff5b365,
                0xfff5b365,
                0xffe68c36,
                0xffdcac46,
                0xffffffff,
                0xffeee7d4,
                0xffe9e2c5,
                0xffebdcbb,
                0xfff0e0a5,
                0xffe0c78e,
                0xffcda87c,
                0xfff9d575,
                0xffe44434,
                0xffe69077,
                0xffe79c7d,
                0xffc94b4e,
                0xffe6b0a6,
                0xffe4c8c7,
                0xfff2b0a2,
                0xffDA6C6D,
                0xffE2BCAF,
                0xffBD7E75,
                0xffD1C7C5,
                0xff975B43,
                0xffE6A57F,
                0xff7B5E4E,
                0xffAC9188,
                0xffD7967A,
                0xffE6C9BB,
                0xff934165,
                0xffD39EB0,
                0xff644974,
                0xff7F658A,
                0xffA793AE,
                0xffC5057C,
                0xff710083,
                0xff00667F,
                0xff00A0BA,
                0xff53B7C6,
                0xffA1B1EF,
                0xff7F8CC0,
                0xff5960A8,
                0xffC1589A,
                0xffA34FAF,
                0xffD08E54,
                0xff939BD9,
                0xffEA4F00,
                0xffD22D33,
                0xffFDE217,
                0xffFFA4DD,
                0xffCB83D5,
                0xff895D5E,
                0xff02FF19,
                0xff019678,
                0xff9EC400,
                0xffA34402,
                0xff714214,
                0xffFFC3C3,
                0xff855FA8,
                0xff7E6E4A,
                0xff3A3321,
                0xff3D2F22,
                0xff422C2E,
                0xff453E38,
                0xff332c4d,
                0xff515267,
                0xff6c72a2,
                0xff8b93ba,
                0xffa6d5db,
                0xffa4b1c2,
                0xffc5bcd1,
                0xffd8aec5,
                0xffcedada,
                0xffd2d6d5,
                0xffcacccb,
                0xffdfdedc,
                0xff5d5b44,
                0xff726b3d,
                0xffcdcc7c,
                0xff8fd1dd,
                0xff0B0C11,
                0xfff5b365,
                0xffE36664,
                0xff456DFF,
                0xff5FBD72,
                0xfff4d32e,
                0xffa97435,
                0xff41281b,
                0xff41281b,
                0xffa97435,
                0xff34671e,
                0xfff3f3f3,
                0xfff3f3f3,
                0xff34671e,
                0xff7739a7,
                0xfff1c232,
                0xfff1c232,
                0xff7739a7,
                0xff5aa4ad,
                0xffffe9d4,
                0xffffe9d4,
                0xff5aa4ad,
                0xff3a6298,
                0xffd9d9d9,
                0xffd9d9d9,
                0xff3a6298,
                0xff830808,
                0xfff4ca14,
                0xfff4ca14,
                0xff830808,
                0xff2C4D86,
                0xff955066,
                0xff6c1512,
                0xff211f1f,
                0xffccc4bf,
                0xffef9b9b,
                0xffb5d0fd,
                0xffa8ceab,
                0xff8d5c44,
                0xffe9a74d,
                0xffb3a491,
                0xff5f4f44
            };

            public static Color White => new Color(1f, 1f, 1f);

            public static Color Black => new Color(0f, 0f, 0f);

            public static Color Gray => new Color(0.5f, 0.5f, 0.5f);

            public static Color Red => new Color(1f, 0f, 0f);

            public static Color RoyalRed => new Color(155f, 28f, 49f);

            public static Color Green => new Color(0f, 1f, 0f);

            public static Color Blue => new Color(0f, 0f, 1f);

            public static Color Yellow => new Color(1f, 1f, 0f);

            public static Color Purple => new Color(1f, 0f, 1f);

            public static Color Aqua => new Color(0f, 1f, 1f);

            public static Color Orange => new Color(1f, 0.5f, 0f);

            public static Color LimeGreen => new Color(0.5f, 1f, 0f);

            public static Color SkyBlue => new Color(0f, 0.5f, 1f);
        }
    }
}