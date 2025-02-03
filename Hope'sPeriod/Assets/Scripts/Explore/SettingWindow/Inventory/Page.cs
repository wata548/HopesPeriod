using System;
using System.Collections.Generic;
using System.Linq;

public static class Page {

    private static int maxPage;
    private static int currentPage = 0;
    private static CodeType category;
    
    private const int ItemCount = 15;
    private const int LineCount = 5;
    
    public static void CountPage(CodeType category) {

        Page.category = category;
        int count = Inventory.Kind(category);
        maxPage = count / ItemCount + Convert.ToInt32((count % ItemCount != 0));
    }

    public static List<int> Factors() {

        return Inventory.Category(category)
            .Skip(currentPage * ItemCount)
            .Take(ItemCount)
            .ToList();
    }
}