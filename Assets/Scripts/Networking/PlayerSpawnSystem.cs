using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();

    }

    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public override void OnStartServer() => MyNetworkManagerLobby.OnServerReadied += SpawnPlayer;

    [ServerCallback]

    private void OnDestroy() => MyNetworkManagerLobby.OnServerReadied -= SpawnPlayer;

    [Server]

    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }
        Debug.Log($"Spawning Connection: {conn} at {spawnPoints[nextIndex].position} gamejbect: {conn.identity.gameObject}");

        //GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);

        ////NetworkServer.ReplacePlayerForConnection(conn, playerInstance);

        //conn.identity.gameObject.transform.position = spawnPoints[nextIndex].position;
        RpcPositionPlayersAtSpawn(conn.identity.gameObject, spawnPoints[nextIndex].position);
        conn.identity.gameObject.name = spawnPoints[nextIndex].position.ToString();
        //NetworkServer.Spawn(playerInstance, conn);

        nextIndex++;
    }

    [ClientRpc]

    private void RpcPositionPlayersAtSpawn(GameObject player, Vector3 pos)
    {
        player.transform.position = pos;
    }
}
