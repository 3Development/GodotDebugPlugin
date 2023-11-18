using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace WritePlugins.addons.DebugTool.Utilz;

public class RedisController
{
    private readonly ConnectionMultiplexer _multiplexer;
    private readonly IDatabase _db;
    

    public RedisController()
    {
        this._multiplexer = ConnectionMultiplexer.Connect("localhost");
        this._db = _multiplexer.GetDatabase();
    }

    public void UpdatePosition(Node3D node3D)
    {
        _db.StringSet(node3D.Name.ToString(),node3D.Position.X.ToString());
    }

    
    /**
     * 
     */
    public void SetInitialPositionsForAllNodesInScene(PackedScene scene)
    {
        IDictionary<string, Vector3> positions = new Dictionary<string, Vector3>();
        
        foreach (Node3D child in scene.GetLocalScene().GetChildren())
        {
            positions[child.Name] = child.Position;
        } 
        var json =  JsonConvert.SerializeObject( positions );

        _db.StringSet("GameInitPosition", json);
    }
    
    /**
     * 
     */
    public IDictionary<string, Vector3> GetInitialPositions()
    {
        return JsonConvert.DeserializeObject<Dictionary<string, Vector3>>(_db.StringGet("GameInitPosition"));
    }


    public Vector3 GetPosition(Node3D node3D)
    {
        string value = _db.StringGet(node3D.Name.ToString());
        
        return value == null  ? node3D.Position : new Vector3(
                                                    float.Parse(value),
                                                    node3D.Position.Y,
                                                    node3D.Position.Z
                                                );
    }

    private void ClearAllKeys()
    {
        var server = _multiplexer.GetServers()[0];
        var db = _multiplexer.GetDatabase();
        var keys = server.Keys();
        
        foreach (var key in keys)
        {
            db.KeyDelete(key);
        }
    }
}