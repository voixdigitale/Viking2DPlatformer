using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(menuName = "2D/Tiles/Custom RuleTile")]
public class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor> {
    public bool alwaysConnect;
    public TileBase[] tilesToConnect;
    public bool checkSelf;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Any = 3;
        public const int Specific = 4;
        public const int Nothing = 5;

    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This:
                return Check_This(tile);
            case Neighbor.NotThis:
                return tile != this;
            case Neighbor.Any:
                return Check_Any(tile);
            case Neighbor.Specific:
                return Check_Specific(tile);
            case Neighbor.Nothing:
                return Check_Nothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    public bool Check_This(TileBase tile) {
        if (!alwaysConnect)
            return tile == this;
        else
            return tilesToConnect.Contains(tile) || tile == this;
    }

    public bool Check_Any(TileBase tile) {
        if (checkSelf)
            return tile != null;
        else
            return tile != null && tile != this;
    }

    public bool Check_Specific(TileBase tile) {
        return tilesToConnect.Contains(tile) || tilesToConnect != null;
    }

    public bool Check_Nothing(TileBase tile) {
        return tile == null;
    }
}