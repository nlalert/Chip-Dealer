
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
            ChipyChip0,
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

            _price.Add(RelicType.PotatoChip, 200);
            _price.Add(RelicType.ProcessingChip, 50);
            _price.Add(RelicType.ChippedChip, 50);
            _price.Add(RelicType.ExplosiveChip, 100);
            _price.Add(RelicType.ChipyChip0, 5);
            _price.Add(RelicType.ChipyChip1, 5);
            _price.Add(RelicType.ChipyChip2, 5);
            _price.Add(RelicType.ChipyChip3, 5);
            _price.Add(RelicType.ChipyChip4, 5);
            _price.Add(RelicType.ChipyChip5, 10);
            _price.Add(RelicType.ChipyChip6, 10);
            _price.Add(RelicType.ChipyChip7, 10);
            _price.Add(RelicType.ChipyChip8, 20);

            return _price[relicType];
      }

      public static int GetRelicRarity(RelicType relicType)
      {

            Dictionary<RelicType, int> _rarity = new Dictionary<RelicType, int>();

            _rarity.Add(RelicType.PotatoChip, 3);
            _rarity.Add(RelicType.ProcessingChip, 1);
            _rarity.Add(RelicType.ChippedChip, 1);
            _rarity.Add(RelicType.ExplosiveChip, 2);
            _rarity.Add(RelicType.ChipyChip0, 0);
            _rarity.Add(RelicType.ChipyChip1, 0);
            _rarity.Add(RelicType.ChipyChip2, 0);
            _rarity.Add(RelicType.ChipyChip3, 0);
            _rarity.Add(RelicType.ChipyChip4, 0);
            _rarity.Add(RelicType.ChipyChip5, 1);
            _rarity.Add(RelicType.ChipyChip6, 1);
            _rarity.Add(RelicType.ChipyChip7, 1);
            _rarity.Add(RelicType.ChipyChip8, 2);

            return _rarity[relicType];
      }

      public static String GetRelicDescriptions(RelicType relicType)
      {

            Dictionary<RelicType, String> _descriptions = new Dictionary<RelicType, String>();

            _descriptions.Add(RelicType.PotatoChip, "Does nothing.");
            _descriptions.Add(RelicType.ProcessingChip, "Increase aim\nguide line by 50%.");
            _descriptions.Add(RelicType.ChippedChip, "Items in shop\nare 20% cheaper.");
            _descriptions.Add(RelicType.ExplosiveChip, "explode and destroy\nsurrounding chips");
            _descriptions.Add(RelicType.ChipyChip0, "Does nothing.");
            _descriptions.Add(RelicType.ChipyChip1, "Does nothing.");
            _descriptions.Add(RelicType.ChipyChip2, "Does nothing.");
            _descriptions.Add(RelicType.ChipyChip3, "Does nothing.");
            _descriptions.Add(RelicType.ChipyChip4, "Does nothing.");
            _descriptions.Add(RelicType.ChipyChip5, "Uncommon \nDoes nothing.");
            _descriptions.Add(RelicType.ChipyChip6, "Uncommon \nDoes nothing.");
            _descriptions.Add(RelicType.ChipyChip7, "Uncommon \nDoes nothing.");
            _descriptions.Add(RelicType.ChipyChip8, "Rare.\nDoes nothing.");


            return _descriptions[relicType];
      }

}

