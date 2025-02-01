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
                "RGBYPWOR",
                "PWORGBY-",
                "RGBYPWOR",
                "PWORGBY-"
            }
        },
        {
            17, new[]
            {
                "RGR--YBY",
                "BYB-GRG-",
                "RGR--YBY",
                "BYB-GRG-",
                "RGR--YBY"
            }
        },
        {
            18, new[]
            {
                "----W---",
                "---W----",
                "-G--W-K-",
                "P--W--G-",
                "-B--W-P-",
                "-R-W-B--",
                "--RBKR--"
            }
        },
        {
            19, new[]
            {
                "WGYWGYWG",
                "YWG-WGY-",
                "-GY--YW-",
                "-W---G--",
                "-GY--YW-",
                "YWG-WGY-",
                "WGYWGYWG",
            }
        },
        {
            20, new[]
            {
                "Y-W--K-R",
                "B-G-K-Y-",
                "Y--PW--R",
                "R-KWB-Y-",
                "Y--WW--R",
                "B-K-P-Y-",
                "Y-K--K-R",
                "R-----Y-"
            }
        },
        {
            21, new[]
            {
                "WPOYWPOY",
                "OYW-OYW-",
                "-PO--PO-",
                "-Y---Y--",
                "-PO--PO-",
                "OYW-OYW-",
                "WPOYWPOY"
            }
        },
        {
            22, new[]
            {
                "RRRRRRRR",
                "Y-------",
                "-GBPORYG",
                "-P------",
                "--OYGBPO",
                "--R-----",
                "---YGBPO"
            }
        },
        {
            23, new[]
            {
                "---PG---", 
                "--BPW---",
                "--GBWR--",
                "-YG-RO--",
                "-OY--OY-",
                "RO---YG-",
                "WR----GB",
                "W-----B-"
            }
        },
        {
            24, new[]
            {
                "WROY----",
                "PBG-----",
                "WROY----",
                "PBG-----",
                "WROY----",
                "PBG-----"
            }
        },
        {
            25, new[]
            {
                "B------P",
                "B-----P-",
                "-K----R-",
                "RBYGOPK-",
                "-K----R-",
                "RBYGOPK-",
                "-K----R-",
                "RBYGOPK-"
            }
        },
        {
            26, new[]
            {
                "RGBYPOWK",
                "OWKRGBY-",
                "RGBYPOWK",
                "OWKRGBY-"
            }
        },
        {
            27, new[]
            {
                "RRR--RRR",
                "--R-R---",
                "--BGGB--",
                "-G-P-G--",
                "--PGGP--",
                "--RKR---",
                "-YB--BY-"
            }
        },
        {
            28, new[]
            {
                "RGBWYRG-",
                "WYRGBW--",
                "-GBWYR--",
                "-YRGB---",
                "--BWY---",
                "--RG----",
                "---W----"
            }
        },
        {
            29, new[]
            {
                "--G--G--",
                "--G-G---",
                "---GG---",
                "-PRGRP--",
                "-GY--YG-",
                "W-RBR-W-",
                "K--PP--K"
            }
        },
        {
            30, new[]
            {
                "R-B-R-B-",
                "G-Y-G-Y-",
                "B-P-B-P-",
                "Y-O-Y-O-",
                "P-W-P-W-",
                "O-K-O-K-",
                "W-R-W-R-"
            }
        },
        //Debug Stage
        {
            999, new[]
            {
                "---K-K--",
                "--K-K---",
            }
        },
        //Debug Stage
        {
            1000, new[]
            {
                "-----K-K",
                "-----KK-",
                "-----Y-Y",
                "------B-",
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
