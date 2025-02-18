using m2ostnextservice.Models;
using m2ostnextservice;
using System.Collections.Generic;

public class MyDashboardJSONResponce
{
    public FootballThemeLeaderBoardHeader header;
    public LeaderBoardResponse Leader;
    public List<tbl_badge_master> badgemaster;
    public tbl_profile prof;
    public int totalcurrency;
}