using System;
using System.Collections.Generic;
using System.Linq;

public class DiaryEntry
{
    public int id { get; set; }
    public string entrydate { get; set; }
    public int feeling { get; set; }
    public string entry { get; set; }
}

public class Diary
{
    public List<DiaryEntry> entries { get; set; }

    #region DiaryFunctions
    // Legt einen Tagebucheintrag an
    public void addDiaryEntry(DiaryEntry diaryEntry)
    {
        if (entries == null)
        {
            entries = new List<DiaryEntry>();
        }
        entries.Add(diaryEntry);
    }

    // Gibt alle Tagebucheintr�ge f�r einen bestimmten Tag zur�ck
    public List<DiaryEntry> getDiaryEntriesByDate(DateTime date)
    {
        return entries.Where(entry => DateTime.Parse(entry.entrydate) == date).ToList();
    }

    // Gibt eine Liste aller Tagebucheintr�ge zur�ck, die im Zeitraum liegen
    public List<DiaryEntry> getDiaryEntriesByTimeFrame(DateTime fromDate, DateTime toDate)
    {
        return entries.Where(entry => DateTime.Parse(entry.entrydate) >= fromDate && DateTime.Parse(entry.entrydate) <= toDate).ToList();
    }

    // Gibt eine Liste aller Tagebucheintr�ge zur�ck, die von einem der �bergebenen Gef�hlslevel sind
    public List<DiaryEntry> getDiaryEntriesByFeeling(List<int> feelings)
    {
        return entries.Where(entry => feelings.Contains(entry.feeling)).ToList();
    }

    // Gibt eine Liste aller Tagebucheintr�ge zur�ck, die im Zeitraum liegen und eines der �bergebenen Gef�hlslevel haben
    public List<DiaryEntry> GetDiaryEntriesByDateAndFeeling(DateTime fromDate, DateTime toDate, List<int> feelings)
    {
        return entries.Where(entry => DateTime.Parse(entry.entrydate) >= fromDate && DateTime.Parse(entry.entrydate) <= toDate && feelings.Contains(entry.feeling)).ToList();

    }

    // Gibt neuesten DiaryEntry zur�ck
    public DiaryEntry getLastEntry()
    {
        return entries.OrderByDescending(entry => entry.entrydate).FirstOrDefault();
    }
    #endregion
}