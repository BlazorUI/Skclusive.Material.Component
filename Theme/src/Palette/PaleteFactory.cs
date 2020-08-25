using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Skclusive.Material.Theme
{
    public static class PaletteFactory
    {
        public static PaletteColor AugmentColor(PaletteColorConfig color,
            Func<PaletteColorConfig, string> mainShade,
            Func<PaletteColorConfig, string> lightShade,
            Func<PaletteColorConfig, string> darkShade,
            decimal tonalOffset = 0.2m,
            decimal contrastThreshold = 3)
        {
            var main = color.Main ?? mainShade(color);

            var light = color.Light ?? lightShade(color) ?? main.Lighten(tonalOffset);

            var dark = color.Dark ?? darkShade(color) ?? main.Darken(tonalOffset * 1.5m);

            var contrastText = color.ContrastText ?? ToContrastText(main, contrastThreshold);

            var custom = color.Custom ?? new Dictionary<string, string>();

            return new PaletteColor
            {
                Main = main,

                Light = light,

                Dark = dark,

                ContrastText = contrastText,

                Custom = custom,
            };
        }

        public static PaletteColor AugmentColor(PaletteColorConfig colorConfig, decimal tonalOffset = 0.2m, decimal contrastThreshold = 3)
        {
            return AugmentColor(colorConfig, p => p.X500, p => p.X300, p => p.X700, tonalOffset, contrastThreshold);
        }

        public static string ToContrastText(string background, decimal contrastThreshold = 3)
        {
            return background.ContrastRatio(TypeColors.Dark.Text.Primary) >= contrastThreshold ? TypeColors.Dark.Text.Primary : TypeColors.Light.Text.Primary;
        }

        private static CommonColors ToCommon(CommonColors input, CommonColors defaultx)
        {
            return new CommonColors
            {
                White = input?.White ?? defaultx.White,

                Black = input?.Black ?? defaultx.Black,

                Custom = input?.Custom ?? defaultx.Custom,
            };
        }

        private static TypeColor ToColor(TypeColor input, TypeColor defaultx)
        {
            return new TypeColor
            {
                X50 = input?.X50 ?? defaultx.X50,

                X100 = input?.X100 ?? defaultx.X100,

                X200 = input?.X200 ?? defaultx.X200,

                X300 = input?.X300 ?? defaultx.X300,

                X400 = input?.X400 ?? defaultx.X400,

                X500 = input?.X500 ?? defaultx.X500,

                X600 = input?.X600 ?? defaultx.X600,

                X700 = input?.X700 ?? defaultx.X700,

                X800 = input?.X800 ?? defaultx.X800,

                X900 = input?.X900 ?? defaultx.X900,

                A100 = input?.A100 ?? defaultx.A100,

                A200 = input?.A200 ?? defaultx.A200,

                A400 = input?.A400 ?? defaultx.A400,

                A700 = input?.A700 ?? defaultx.A700,
            };
        }

        private static TypeBackground ToBackground(TypeBackground input, TypeBackground defaultx)
        {
            return new TypeBackground
            {
                Paper = input?.Paper ?? defaultx.Paper,

                Default = input?.Default ?? defaultx.Default,

                Custom = input?.Custom ?? defaultx.Custom,
            };
        }

        private static TypeCustom ToCustom(TypeCustom input, TypeCustom defaultx)
        {
            return new TypeCustom
            {
                LightOrDark = input?.LightOrDark ?? defaultx.LightOrDark,

                LightOrDarkContrastText = input?.LightOrDarkContrastText ?? defaultx.LightOrDarkContrastText,

                ContentBackground = input?.ContentBackground ?? defaultx.ContentBackground,

                ContentBackgroundDefault = input?.ContentBackgroundDefault ?? defaultx.ContentBackgroundDefault,

                PaletteOpacity = input?.PaletteOpacity ?? defaultx.PaletteOpacity,

                PaletteCommonAlternate = input?.PaletteCommonAlternate ?? defaultx.PaletteCommonAlternate,

                LayoutBackward = input?.LayoutBackward ?? defaultx.LayoutBackward,

                LayoutForward = input?.LayoutForward ?? defaultx.LayoutForward,

                PrimaryMain = input?.PrimaryMain ?? defaultx.PrimaryMain,

                PrimaryContrastText = input?.PrimaryContrastText ?? defaultx.PrimaryContrastText,
            };
        }

        private static TypeAction ToAction(TypeAction input, TypeAction defaultx)
        {
            return new TypeAction
            {
                Active = input?.Active ?? defaultx.Active,

                Hover = input?.Hover ?? defaultx.Hover,

                HoverOpacity = input?.HoverOpacity ?? defaultx.HoverOpacity,

                Selected = input?.Selected ?? defaultx.Selected,

                Disabled = input?.Disabled ?? defaultx.Disabled,

                DisabledBackground = input?.DisabledBackground ?? defaultx.DisabledBackground,

                Custom = input?.Custom ?? defaultx.Custom,
            };
        }

        private static TypeText ToText(TypeText input, TypeText defaultx)
        {
            return new TypeText
            {
                Primary = input?.Primary ?? defaultx.Primary,

                Secondary = input?.Secondary ?? defaultx.Secondary,

                Disabled = input?.Disabled ?? defaultx.Disabled,

                Hint = input?.Hint ?? defaultx.Hint,

                Custom = input?.Custom ?? defaultx.Custom,
            };
        }

        public static Palette CreatePalette(PaletteConfig config)
        {
            var tonalOffset = config?.TonalOffset ?? 0.2m;

            var contrastThreshold = config?.ContrastThreshold ?? 3;

            var primary = AugmentColor(config?.Primary ?? new PaletteColorConfig
            {
                Light = TypeColors.Indigo.X300,

                Main = TypeColors.Indigo.X500,

                Dark = TypeColors.Indigo.X700

            }, tonalOffset, contrastThreshold);

            var secondary = AugmentColor(config?.Secondary ?? new PaletteColorConfig
            {
                Light = TypeColors.Pink.A200,

                Main = TypeColors.Pink.A400,

                Dark = TypeColors.Pink.A700
            },
            p => p.A400, p => p.A200, p => p.A700, tonalOffset, contrastThreshold);

            var error = AugmentColor(config?.Error ?? new PaletteColorConfig
            {
                Light = TypeColors.Red.X300,

                Main = TypeColors.Red.X500,

                Dark = TypeColors.Red.X700

            }, tonalOffset, contrastThreshold);

            var type = config?.Type ?? PaletteType.Light;

            var isLight = type == PaletteType.Light;

            var palette = new Palette
            {
                Primary = primary,

                Secondary = secondary,

                Error = error,

                Type = type,

                ContrastThreshold = contrastThreshold,

                TonalOffset = tonalOffset,

                Divider = config?.Divider ?? (isLight ? TypeColors.Light.Divider : TypeColors.Dark.Divider),

                Grey = ToColor(config?.Grey, TypeColors.Grey),

                Common = ToCommon(config?.Common, TypeColors.Common),

                Text = ToText(config?.Text, (isLight ? TypeColors.Light.Text : TypeColors.Dark.Text)),

                Background = ToBackground(config?.Background, (isLight ? TypeColors.Light.Background : TypeColors.Dark.Background)),

                Action = ToAction(config?.Action, (isLight ? TypeColors.Light.Action : TypeColors.Dark.Action)),

                Custom = ToCustom(config?.Custom, (isLight ? TypeColors.Light.Custom : TypeColors.Dark.Custom)),
            };

            return palette;
        }
    }
}