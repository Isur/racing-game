using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Save {
    private static List<string> FinishedRaces = new List<string>();
    public static List<string> load(){
        loadFromFile();
        return FinishedRaces;
    }
    
    public static void update(string race){
        if(!FinishedRaces.Exists(x => x == race)){
            FinishedRaces.Add(race);
        }
        saveToFile();
    }

    private static void saveToFile(){
        /*
            TODO:
            Save to to file List<string> FinishedRaces.
         */
    }

    private static void loadFromFile(){
        /*
            TODO:
            Load from file to List<string> FinishedRaces
         */
    }
}
