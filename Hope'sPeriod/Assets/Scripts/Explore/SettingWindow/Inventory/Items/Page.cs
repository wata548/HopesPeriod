using System;
using System.Collections.Generic;
using System.Linq;

public static class Page {

    public static int MaxPage { get; private set; }
    public static int CurrentPage { get; private set; } = 0;
    private static CodeType category;
    
    private const int ItemCount = 15;
    private const int LineCount = 5;

    public static void Refresh() {
        int count = Inventory.Kind(category);
        MaxPage = count / ItemCount + Convert.ToInt32((count % ItemCount != 0));
    }
    
    public static void CountPage(CodeType category) {

        Page.category = category;
        Refresh();
    }

    public static void NextPage() {
        CurrentPage++;
        if (CurrentPage >= MaxPage) CurrentPage = 0;
    }

    public static void PriviousPage() {
        CurrentPage--;
        if (CurrentPage < 0) CurrentPage = MaxPage - 1;
    } 

    public static List<int> Factors() {

        return Inventory.Category(category)
            .Skip(CurrentPage * ItemCount)
            .Take(ItemCount)
            .ToList();
    }
}