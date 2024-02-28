using System.Collections.Generic;
using UnityEngine;

public class EndlessTrackGenerator : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TrackSegment firstTrackPrefab;
    [SerializeField] private TrackSegment[] easyTrackPrefabs;
    [SerializeField] private TrackSegment[] hardTrackPrefabs;
    [SerializeField] private TrackSegment[] rewardTrackPrefabs;

    [Header("Endless Generation Parameters")]
    [SerializeField] private int initialTrackCount = 5;
    [SerializeField] private int minTracksInFrontOfPlayer = 3;
    [SerializeField] private float minDistanceToConsiderInsideTrack = 3;

    [Header("Level Difficulty Parameters")]
    [Range(0, 1)]
    [SerializeField] private float hardTrackChance = 0.2f;
    [SerializeField] private int minTracksBeforeReward = 10;
    [SerializeField] private int maxTracksBeforeReward = 20;
    [SerializeField] private int minRewardTrackCount = 1;
    [SerializeField] private int maxRewardTrackCount = 3;

    private List<TrackSegment> _currentSegments = new();

    private bool _shouldSpawnRewardTracks;
    private int _rewardTracksLeftToSpawn;
    private int _trackSpawnedAfterLastReward;

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
        var playerTrackIndex = GetTrackIndexWithPlayer();
        if (playerTrackIndex < 0)
        {
            return;
        }

        //Spawn more track is needed
        var tracksInFrontOfPlayer = _currentSegments.Count - (playerTrackIndex + 1);
        if (tracksInFrontOfPlayer < minTracksInFrontOfPlayer)
        {
            SpawnTracks(minTracksInFrontOfPlayer - tracksInFrontOfPlayer);
        }

        //Despawn tracks behind player
        for (var i = 0; i < playerTrackIndex; i++)
        {
            Destroy(_currentSegments[i].gameObject);
        }
        _currentSegments.RemoveRange(0, playerTrackIndex);
    }

    private int GetTrackIndexWithPlayer()
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

    private void SpawnTracks(int trackCount)
    {
        var previousTrack = _currentSegments.Count > 0 
            ? _currentSegments[^1] 
            : null;
        
        for (var i = 0; i < trackCount; i++)
        {
            var track = GetRandomTrack();
            previousTrack = SpawnTrackSegment(track, previousTrack);
        }
    }

    private TrackSegment GetRandomTrack()
    {
        TrackSegment[] trackList = null;
        
        if (_shouldSpawnRewardTracks)
        {
            trackList = rewardTrackPrefabs;
        }
        else
        {
            trackList = Random.value <= hardTrackChance ? hardTrackPrefabs : easyTrackPrefabs;
        }
        
        return trackList[Random.Range(0, trackList.Length)];
    }

    private TrackSegment SpawnTrackSegment(TrackSegment track, TrackSegment previousTrack)
    {
        var trackInstance = Instantiate(track, transform);

        if (previousTrack != null)
        {
            trackInstance.transform.position = 
                previousTrack.End.position + 
                (trackInstance.transform.position - trackInstance.Start.position);
        }
        else
        {
            trackInstance.transform.position = Vector3.zero;
        }
        
        trackInstance.SpawnObstacles();
        trackInstance.SpawnDecorations();
        trackInstance.SpawnPickups();

        _currentSegments.Add(trackInstance);

        UpdateTrackDifficultyParameters();

        return trackInstance;
    }

    private void UpdateTrackDifficultyParameters()
    {
        if (_shouldSpawnRewardTracks)
        {
            _rewardTracksLeftToSpawn--;
            if (_rewardTracksLeftToSpawn <= 0)
            {
                _shouldSpawnRewardTracks = false;
                _trackSpawnedAfterLastReward = 0;
            }
        }
        else
        {
            _trackSpawnedAfterLastReward++;
            var requiredTracksBeforeReward = Random.Range(minTracksBeforeReward, maxTracksBeforeReward + 1);
            if (_trackSpawnedAfterLastReward >= requiredTracksBeforeReward)
            {
                _shouldSpawnRewardTracks = true;
                _rewardTracksLeftToSpawn = Random.Range(minRewardTrackCount, maxRewardTrackCount + 1);
            }
        }
    }
}