using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrackGenerator : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [Header("Track Prefabs")]
    [Space]
    [SerializeField] private TrackSegment firstTrackPrefab;
    [SerializeField] private TrackSegment[] easyTracksPrefabs;
    [SerializeField] private TrackSegment[] hardTracksPrefabs;
    [SerializeField] private TrackSegment[] rewardTracksPrefabs;
    
    [Header("Endless Generation Parameters")]
    [Space]
    [SerializeField] private int initialTrackCount = 10;
    [SerializeField] private int minTracksInFrontOfPlayer = 3;
    [SerializeField] private float minDistanceToConsiderInsideTrack = 3;
    [SerializeField] private float requiredTracksBeforeReward = 5;

    [Header("Level Difficulty Parameters")]
    [Space]
    [SerializeField, Range(0, 1)] private float hardTrackChance = 0.3f;

    private List<TrackSegment> currentSegments = new List<TrackSegment>();
    private int tracksSpawnedAfterLastReward = 0;

    private void Start()
    {
        SpawnTrackSegment(firstTrackPrefab, null);
        SpawnTracks(initialTrackCount);
    }

    private void Update()
    {
        UpdateTracks();        
    }

    private void UpdateTracks()
    {
        int playerIndexTrack = GetPlayerIndexTrack();

        if (playerIndexTrack < 0)
        {
            return;
        }

        SpawnTracksInfrontOfPlayer(playerIndexTrack);
        DespawnTracksBehindPlayer(playerIndexTrack);
    }

    private void DespawnTracksBehindPlayer(int playerIndexTrack)
    {
        for (var i = 0; i < playerIndexTrack; i++)
        {
            var track = currentSegments[i];
            Destroy(track.gameObject);
        }

        currentSegments.RemoveRange(0, playerIndexTrack);
    }

    private void SpawnTracksInfrontOfPlayer(int playerIndexTrack)
    {
        int tracksInFrontOfPlayer = currentSegments.Count - (playerIndexTrack + 1);
        if (tracksInFrontOfPlayer < minTracksInFrontOfPlayer)
        {
            SpawnTracks(minTracksInFrontOfPlayer - tracksInFrontOfPlayer);
        }
    }

    private int GetPlayerIndexTrack()
    {
        var playerIndexTrack = -1;

        for (var i = 0; i < currentSegments.Count; i++)
        {
            var track = currentSegments[i];
            if (player.transform.position.z >= track.Start.transform.position.z + minDistanceToConsiderInsideTrack &&
                player.transform.position.z <= track.End.transform.position.z)
            {
                playerIndexTrack = i;
                break;
            }
        }

        return playerIndexTrack;
    }

    private void SpawnTracks(int trackCount)
    {
        TrackSegment previousTrack = currentSegments.Count > 0 
            ? currentSegments[currentSegments.Count - 1]
            : null;

        for (var i = 0; i < trackCount; i++)
        {
            TrackSegment trackPrefab = GetRandomTrackPrefab();
            previousTrack = SpawnTrackSegment(trackPrefab, previousTrack);
        }
    }

    private TrackSegment SpawnTrackSegment(TrackSegment track, TrackSegment previousTrack)
    {
        TrackSegment trackInstance = Instantiate(track, transform);        

        if (previousTrack != null)
        {
            trackInstance.transform.position = previousTrack.End.position + (trackInstance.transform.position - trackInstance.Start.position);
        }
        else
        {
            trackInstance.transform.localPosition = Vector3.zero;
        }

        foreach (var obstacleSpawner in trackInstance.ObstacleSpawners)
        {
            obstacleSpawner.Spawn();
        }

        currentSegments.Add(trackInstance);
        
        return trackInstance;
    }

    private TrackSegment GetRandomTrackPrefab()
    {
        TrackSegment[] trackPrefabsList = null;

        if (tracksSpawnedAfterLastReward >= requiredTracksBeforeReward)
        {
            trackPrefabsList = rewardTracksPrefabs;
            tracksSpawnedAfterLastReward = 0;            
        }
        else
        {
            trackPrefabsList = Random.value <= hardTrackChance
                ? hardTracksPrefabs
                : easyTracksPrefabs;
            tracksSpawnedAfterLastReward++;
        }
        
        var index = Random.Range(0, trackPrefabsList.Length);

        return trackPrefabsList[index];
    }
}
