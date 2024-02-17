using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EndlessTrackGenerator : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TrackSegment initialTrackPrefab;
    [SerializeField] private TrackSegment[] easyTrackPrefabs;
    [SerializeField] private TrackSegment[] hardTrackPrefabs;
    [SerializeField] private TrackSegment[] rewardTrackPrefabs;
    
    [Header("Endless Generation Parameters")]
    [SerializeField] private int initialTrackCount = 10;
    [SerializeField] private int minTracksInFrontOfPlayer = 3;
    [SerializeField] private float minDistanceToConsiderInsideTrack = 3;
    
    [Header("Level Difficulty Parameters")]
    [SerializeField, Range(0, 1)] private float hardTrackChance = 0.3f;
    [SerializeField] private int minTracksBeforeReward = 5;

    private List<TrackSegment> _currentSegments = new();
    private bool _shouldSpawnRewardTracks;
    private int _regularTracksSpawnedAfterLastReward;

    private void Start()
    {
        SpawnTrackSegment(initialTrackPrefab, null);
        SpawnTracks(initialTrackCount);
    }

    private void Update()
    {
        UpdateTracks();
    }

    private void UpdateTracks()
    {
        var playerTrackIndex = GetPlayerTrackIndex();

        if (playerTrackIndex < 0) return;
        
        SpawnTracksInFrontOfPlayer(playerTrackIndex);
        RemoveTracksBehindPlayer(playerTrackIndex);
    }

    private void RemoveTracksBehindPlayer(int playerTrackIndex)
    {
        for (var i = 0; i < playerTrackIndex; i++)
        {
            var track = _currentSegments[i];
            Destroy(track.gameObject);
        }

        _currentSegments.RemoveRange(0, playerTrackIndex);
    }

    private void SpawnTracksInFrontOfPlayer(int playerTrackIndex)
    {
        var tracksInFrontOfPlayer = _currentSegments.Count - (playerTrackIndex + 1);
        if (tracksInFrontOfPlayer < minTracksInFrontOfPlayer)
        {
            SpawnTracks(minTracksInFrontOfPlayer - tracksInFrontOfPlayer);
        }
    }

    private int GetPlayerTrackIndex()
    {
        for (var i = 0; i < _currentSegments.Count; i++)
        {
            var track = _currentSegments[i];
            if (player.transform.position.z >= track.Start.position.z + minDistanceToConsiderInsideTrack &&
                player.transform.position.z <= track.End.position.z)
            {
                return i;
            }
        }

        return -1;
    }

    private TrackSegment GetRandomTrackPrefab()
    {
        TrackSegment[] prefabList = null;
        
        if (_shouldSpawnRewardTracks)
        {
            prefabList = rewardTrackPrefabs;
        }
        else
        {
            prefabList = Random.value < hardTrackChance 
                ? hardTrackPrefabs 
                : easyTrackPrefabs;
        }

        return prefabList[Random.Range(0, prefabList.Length)];
    }

    private void SpawnTracks(int trackCount)
    {
        var previousTrack = _currentSegments.Count > 0 
            ? _currentSegments[^1] 
            : null;
        
        for (var i = 0; i < trackCount; i++)
        {
            previousTrack = SpawnTrackSegment(GetRandomTrackPrefab(), previousTrack);
        }
    }

    private TrackSegment SpawnTrackSegment(TrackSegment trackPrefab, TrackSegment previousTrack)
    {
        var trackInstance = Instantiate(trackPrefab, transform);

        if (previousTrack != null)
        {
            trackInstance.transform.position = previousTrack.End.position +
                                               (trackInstance.transform.position -
                                                trackInstance.Start.position);
        }
        else
        {
            trackInstance.transform.localPosition = Vector3.zero;
        }

        trackInstance.SpawnObstacles();
        _currentSegments.Add(trackInstance);

        UpdateRewardTracks();

        return trackInstance;
    }

    private void UpdateRewardTracks()
    {
        if (_regularTracksSpawnedAfterLastReward >= minTracksBeforeReward)
        {
            _shouldSpawnRewardTracks = true;
            _regularTracksSpawnedAfterLastReward = 0;
        }
        else
        {
            _shouldSpawnRewardTracks = false;
            _regularTracksSpawnedAfterLastReward++;
        }
    }
}
