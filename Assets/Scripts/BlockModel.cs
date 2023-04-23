using System;
using UnityEngine;

[Serializable]
public class BlockModel
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string subject;
    [SerializeField]
    private string grade;
    [SerializeField]
    private int mastery;
    [SerializeField]
    private string domainid;
    [SerializeField]
    private string domain;
    [SerializeField]
    private string cluster;
    [SerializeField]
    private string standardid;
    [SerializeField]
    private string standarddescription;

    public int Id { get => id; }
    public string Grade { get => grade; }
    public int Mastery { get => mastery; }
    public string Domain { get => domain; }
    public string Cluster { get => cluster; }
    public string Standardid { get => standardid; }
    public string StandardDescription { get => standarddescription; }
}