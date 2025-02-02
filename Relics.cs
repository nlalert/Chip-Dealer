
using System;
using System.Collections.Generic;
using System.Linq;
public class Relics
{
      public enum RelicType
      {
            None,
            PotatoChip,
            ProcessingChip,
            ChippedChip,
            ExplosiveChip,
            ChipyChip,
            ChipyChip1,
            ChipyChip2,
            ChipyChip3,
            ChipyChip4,
            ChipyChip5,
            ChipyChip6,
            ChipyChip7,
            ChipyChip8,
      }

      public static RelicType GetRandomRelic()
      {
           return (RelicType)Singleton.Instance.Random.Next(1,Enum.GetValues(typeof(RelicType)).Length);
      }

      public static List<RelicType> GetEmptyRelicList()
      {
           List<RelicType> EmptyList = Enumerable.Repeat(RelicType.None, 9).ToList();
           return EmptyList;
      }

      public static int GetRelicPrice(RelicType relicType)
      {

            Dictionary<RelicType, int> _price = new Dictionary<RelicType, int>();

            _price.Add(RelicType.PotatoChip, 20);
            _price.Add(RelicType.ProcessingChip, 15);
            _price.Add(RelicType.ChippedChip, 50);
            _price.Add(RelicType.ExplosiveChip, 10);
            _price.Add(RelicType.ChipyChip, 5);
            _price.Add(RelicType.ChipyChip1, 5);
            _price.Add(RelicType.ChipyChip2, 5);
            _price.Add(RelicType.ChipyChip3, 5);
            _price.Add(RelicType.ChipyChip4, 5);
            _price.Add(RelicType.ChipyChip5, 5);
            _price.Add(RelicType.ChipyChip6, 5);
            _price.Add(RelicType.ChipyChip7, 5);
            _price.Add(RelicType.ChipyChip8, 5);

            return _price[relicType];
      }

      public static int GetRelicRarity(RelicType relicType)
      {

            Dictionary<RelicType, int> _rarity = new Dictionary<RelicType, int>();

            _rarity.Add(RelicType.PotatoChip, 0);
            _rarity.Add(RelicType.ProcessingChip, 1);
            _rarity.Add(RelicType.ChippedChip, 2);
            _rarity.Add(RelicType.ExplosiveChip, 0);
            _rarity.Add(RelicType.ChipyChip, 0);
            _rarity.Add(RelicType.ChipyChip1, 0);
            _rarity.Add(RelicType.ChipyChip2, 0);
            _rarity.Add(RelicType.ChipyChip3, 0);
            _rarity.Add(RelicType.ChipyChip4, 0);
            _rarity.Add(RelicType.ChipyChip5, 0);
            _rarity.Add(RelicType.ChipyChip6, 0);
            _rarity.Add(RelicType.ChipyChip7, 0);
            _rarity.Add(RelicType.ChipyChip8, 0);

            return _rarity[relicType];
      }

      public static String GetRelicDescriptions(RelicType relicType)
      {

            Dictionary<RelicType, String> _descriptions = new Dictionary<RelicType, String>();

            _descriptions.Add(RelicType.PotatoChip, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ProcessingChip, "Increase aim\nguide line by 50%.");
            _descriptions.Add(RelicType.ChippedChip, "Items in shop\nare 20% cheaper.");
            _descriptions.Add(RelicType.ExplosiveChip, "explode and destroy\nsurrounding chips");
            _descriptions.Add(RelicType.ChipyChip, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChipyChip1, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChipyChip2, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChipyChip3, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChipyChip4, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChipyChip5, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChipyChip6, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChipyChip7, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChipyChip8, "Does nothing.\nFor now.");


            return _descriptions[relicType];
      }

}

