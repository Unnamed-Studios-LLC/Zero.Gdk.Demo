using System.Text;
using UnityEngine;
using Zero.Demo.Model.Data;

public unsafe class Leaderboard : MonoBehaviour
{
    public LeaderboardListing[] Listings;

    public void SetNames(ref LeaderboardNamesData names)
    {
        fixed (byte* namePtr = &names.Names[0])
        {
            var allNames = Encoding.ASCII.GetString(namePtr, NameData.NameLength * 3);
        }

        for (int i = 0; i < Listings.Length; i++)
        {
            var listing = Listings[i];
            var active = false;
            if (i < LeaderboardNamesData.Count &&
                names.Names[i * NameData.NameLength] != 0)
            {
                active = true;
            }

            listing.gameObject.SetActive(active);
            if (!active)
            {
                continue;
            }

            fixed (byte* namePtr = &names.Names[i * NameData.NameLength])
            {
                var name = Encoding.ASCII.GetString(namePtr, NameData.NameLength);
                listing.Name.text = name;
            }

            var flag = names.GetFlag(i);
            listing.Flag.sprite = CharacterLibrary.GetFlag(flag.Flag);
            listing.FlagColor.color = CharacterLibrary.GetFlagColor(flag.Color);
        }
    }

    public void SetValues(ref LeaderboardValuesData values)
    {
        for (int i = 0; i < Listings.Length; i++)
        {
            var listing = Listings[i];
            listing.Value.text = values.Values[i].ToString();
        }
    }
}
