// DBConnection
// Connect to MySQL
// by Marco Marchesi, 2/3/2016

using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;


public class DBConnection: MonoBehaviour
{

    // MySQL instance specific items
	string constr = "Server=217.114.212.5;Database=thisisma_colopl_test;User ID=thisi_colopl;Password=colopl_testing_2357;Pooling=true";
    // connection object
    MySqlConnection connection = null;
    // command object
    MySqlCommand cmd = null;
    // reader object
    MySqlDataReader dataReader = null;
    // object collection array
    // object definitions
    public struct data
    {
		public string user;
		public string timestamp;
		public string question;
		public int rate;
    }
    // collection container
	private List<data> _Items = new List<data> ();
    void Awake()
    {

		_Items = new List<data>();

        try
        {
            // setup the connection element
            connection = new MySqlConnection(constr);

            // lets see if we can open the connection
            connection.Open();
            Debug.Log("Connection State: " + connection.State);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }

    }

    void OnApplicationQuit()
    {
        Debug.Log("Quit connection");
        if (connection != null)
        {
            if (connection.State.ToString() != "Closed")
                connection.Close();
            connection.Dispose();
        }
    }
		

    // Insert new entries into the table
	public void Insert(string user,string question,int rate)
    {
		data item = new data();
		item.user = user;
		item.timestamp = DateTime.Now.ToString();
		item.question = question;
		item.rate = rate;
		_Items.Add (item);
        string query = string.Empty;
        

		// Get error
        try
        {
            query = "INSERT INTO survey (user,timestamp,question,rate) VALUES (?user, ?timestamp, ?question, ?rate)";
            if (connection.State.ToString() != "Open")
                connection.Open();
            using (connection)
            {
                    using (cmd = new MySqlCommand(query, connection))
                    {
                        MySqlParameter oParam = cmd.Parameters.Add("?user", MySqlDbType.VarString);
                        oParam.Value = item.user;
                        MySqlParameter oParam1 = cmd.Parameters.Add("?timestamp", MySqlDbType.Timestamp);
                        oParam1.Value = item.timestamp;
                        MySqlParameter oParam2 = cmd.Parameters.Add("?question", MySqlDbType.VarString);
                        oParam2.Value = item.question;
                        MySqlParameter oParam3 = cmd.Parameters.Add("?rate", MySqlDbType.Int16);
                        oParam3.Value = item.rate;
                        cmd.ExecuteNonQuery();
                    }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally
        {
        }
    }
		

    // Read all entries from the table
    public void Read()
    {
        string query = string.Empty;
        if (_Items == null)
            _Items = new List<data>();
        if (_Items.Count > 0)
            _Items.Clear();
        
        try
        {
            query = "SELECT * FROM survey";
            if (connection.State.ToString() != "Open")
                connection.Open();
            using (connection)
            {
                using (cmd = new MySqlCommand(query, connection))
                {
                    dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                        while (dataReader.Read())
                        {
                            data item = new data();
                            item.user = dataReader["user"].ToString();
                            item.timestamp = dataReader["timestamp"].ToString();
                            item.question = dataReader["question"].ToString();
							item.rate = int.Parse(dataReader["rate"].ToString());
                            _Items.Add(item);
                        }
                    dataReader.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally
        {
        }
    }

	// log question items
    void LogItems()
    {
        if (_Items != null)
        {
            if (_Items.Count > 0)
            {
                foreach (data item in _Items)
                {
                    Debug.Log("User: " + item.user);
                    Debug.Log("Timestamp: " + item.timestamp);
                    Debug.Log("Question: " + item.question);
                    Debug.Log("Rate: " + item.rate);
                }
            }
        }
    }
		
}