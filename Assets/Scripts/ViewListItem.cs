using System.Collections.Generic;

[System.Serializable]
public class ViewListItem
{
    public string File_Update_Date;
    public int Items_Count;
    public string Bg_List;

    public List<List> List = new List<List>();
}
