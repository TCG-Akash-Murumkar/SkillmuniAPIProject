using m2ostnextservice.Models;
using System.Collections.Generic;

public class LeaderboardJSONResponce
{
    public LeaderBoardResponse Leader;
    public FootballThemeLeaderBoardHeader header;
    public List<tbl_badge_master> badgemaster;
    public string gamename;
}