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
