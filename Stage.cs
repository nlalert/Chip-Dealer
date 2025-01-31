using System;
using System.Collections.Generic;

public class Stage
{
    private static readonly Dictionary<int, string[]> StageLayouts = new Dictionary<int, string[]>
    {
        {
            1, new[]
            {
                "RRYYBBGG",
                "RRYYBBG-",
                "BBGGRRYY",
                "BGGRRYY-"
            }
        },
        {
            2, new[]
            {
                "---KK---",
                "---B----",
                "---G----",
                "---B----",
                "---P----",
                "---G----",
                "---B----",
                "---W----"
            }
        },
        {
            3, new[]
            {
                "G------G",
                "RGBYRGB-",
                "Y------Y",
                "BYRGBYR-",
                "---R----",
                "---G----",
                "---R----"
            }
        },
        {
            4, new[]
            {
                "-KK--BB-",
                "-W---P--",
                "-B---G--",
                "-W---G--",
                "-W---G--",
                "-R---W--",
                "-W---B--",
                "-R---G--"
            }
        },
        {
            5, new[]
            {
                "-Y-Y-Y-P",
                "G-P-B-R-",
                "R-B-Y-P-",
                "-G-Y-B--",
                "-R-P-O--",
                "B-G-G---",
                "--P-Y---",
                "---R----"
            }
        },
        {
            6, new[]
            {
                "RRGRYRPG",
                "G-P-R-R-",
                "-PBRBYW-",
                "-G-Y-R--",
                "-RWYYYR-",
                "W-B-R-R-",
                "GWYYGBGP"
            }
        },
        {
            7, new[]
            {
                "---GY---",
                "--GYG---",
                "---BY---",
                "-PY-OY--",
                "-BOGBPP-",
                "-BG-GB--"
            }
        },
        {
            8, new[]
            {
                "RGBYPWRG",
                "PWRGBYP-",
                "RGBYPWRG",
                "PWRGBYP-"
            }
        },
        {
            9, new[]
            {
                "OOOOOOOO",
                "O-----O-",
                "O--GYKBP",
                "O---BYP-",
                "O-----GR",
                "BK------",
                "GBBY----",
                "RYBYG---"
            }
        },
        {
            10, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            11, new[]
            {
                "---GG---",
                "--BKB---",
                "--YPOO--",
                "-BWYOP--",
                "--OBRW--",
                "--RRB---",
                "---OK---",
                "---Y----"
            }
        },
        {
            12, new[]
            {
                "---RG---",
                "--GBR---",
                "--BRGB--",
                "-RGBRG--",
                "-GBRGBR-",
                "BRGBRGB-",
                "RGBRGBRG"
            }
        },
        {
            13, new[]
            {
                "BBYRRYBB",
                "WBYRYBP-",
                "GGBYYBOO",
                "KWBYBGG-",
                "PKWBBOWK",
                "OPYBROW-",
                "GPGRYPGK"
            }
        },
        {
            14, new[]
            {
                "Y-K--R-Y",
                "B-R-YRP-",
                "-Y-OR-O-",
                "PK---R--",
                "K-BRGO-B",
                "R-----G-",
                "-PKPYPP-"
            }
        },
        {
            15, new[]
            {
                "GG-BB-RR",
                "Y--Y--P-",
                "P--K--K-",
                "K--W--Y-",
                "Y--O--P-",
                "K--O--Y-",
                "P--W--O-",
                "O--Y--P-"
            }
        },
        {
            16, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            17, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            18, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            19, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            20, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            21, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            22, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            23, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            24, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            25, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            26, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            27, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            28, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            29, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            30, new[]
            {
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-",
                "K------R",
                "RBYGOPK-"
            }
        },
        {
            31, new[]
            {
                "---K-K--",
                "--K-K---",
            }
        }
    };

    public static void SetUpBoard()
    {
        if (!StageLayouts.ContainsKey(Singleton.Instance.Stage))
        {
            Console.WriteLine("No more stage : Replaying");
            Singleton.Instance.Stage = 1;
        }

        Console.WriteLine(Singleton.Instance.Stage);

        string[] layout = StageLayouts[Singleton.Instance.Stage];
        int rows = layout.Length;
        int cols = layout[0].Length;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                char chipChar = layout[row][col];
                Singleton.Instance.GameBoard[row, col] = ChipTypeFromChar(chipChar);
            }
        }
    }

    private static ChipType ChipTypeFromChar(char chipChar)
    {
        return chipChar switch
        {
            'R' => ChipType.Red,
            'Y' => ChipType.Yellow,
            'B' => ChipType.Blue,
            'G' => ChipType.Green,
            'P' => ChipType.Purple,
            'W' => ChipType.White,
            'K' => ChipType.Black,
            'O' => ChipType.Orange,
            '-' => ChipType.None,
             _  => ChipType.None
        };
    }
}
