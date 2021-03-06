﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New DT", menuName = "Decision Tree")]
public class DecisionTree : ScriptableObject
{
	public List<DTNode> nodes; 
	// public Dictionary <string, DTNode> genealogy; 

	public string Decide(GSR s)
	{
		string result="None"; 
		int next_node = 0; 
		DTNode current_node = nodes[0]; 
		int counter = 0; 

		while(next_node != -1)
		{
			
			// Debug.Log("Current node: " + current_node.Name + " Counter " + counter.ToString()); 

			current_node.Decide(s, out next_node, out result); 
			// Debug.Log("Next node: " + next_node.ToString()); 
			if(next_node == -1)
				return result; 
			else
				current_node = nodes[next_node]; 

			counter += 1; 
		}
		return result; 
	}
}

[System.Serializable]
public class DTNode
{
	public string Name; 
	public int false_node; 
	public int true_node; 

	public int state_value_to_test; 
	public float test_value; 
	public ProbAction [] actions; 

	public bool decision_type; 

	public DTNode()
	{

	}

	public void Decide(GSR s, out int next_node , out string result)
	{
		if(decision_type)
		{
			next_node = s.state[state_value_to_test] > test_value ? true_node : false_node; 
			result = "None"; 
		}
		else
		{
			next_node = -1; 
			result = Sample(); 
		}
	}

	public void Normalize()
	{
		float sum_prob = 0f; 
		foreach(ProbAction p in actions)
		{
			sum_prob += p.prob; 
		}

		float cum_sum = 0f; 
		for (int i = 0; i<actions.Length; i++) 
		{
			actions[i].prob /= sum_prob; 
			actions[i].prob += cum_sum;
			cum_sum = actions[i].prob; 
		}
	}
	
	public string Sample()
	{
		float selector = Random.Range(0f,1f); 
		foreach(ProbAction p in actions)
		{
			if(selector<p.prob)
				return p.decision_id; 
		}

		return "None"; 
	}

	public override string ToString()
	{
		string recap = Name; 
		if(decision_type && true_node != null) 
			recap += "  True node: " + true_node +  " False node: " + false_node;  
		return recap;
	}
}

[System.Serializable]
public class DecisionTreeNode
{
	public DecisionTreeNode false_node; 
	public DecisionTreeNode true_node; 

	public int state_value_to_test; 
	public float test_value; 
	public ProbAction [] actions; 

	public DecisionTreeNode Decide(GSR s)
	{
		return null; 
	}

	public string Sample()
	{	
		return "Error"; 
	}


}

[System.Serializable]
public class Decision : DecisionTreeNode
{
	public DecisionTreeNode true_node; 
	public DecisionTreeNode false_node; 

	public int state_value_to_test; 
	public float test_value; 

	public DecisionTreeNode Decide(GSR state_reprez)
	{
		float state_value = state_reprez.state[state_value_to_test]; 
		if(test_value > state_value)
		{
			Debug.Log("Selecting true node");
			return true_node; 
		}
		else
		{
			Debug.Log("Selecting false node");
			return false_node; 
		}
	}

	public string Sample()
	{
		return "Error: Decision sample called"; 
	}

	public string ToString()
	{
		return "Decision node testing value " + state_value_to_test.ToString() + " with " + test_value.ToString() + " as limit";
	}
}

[System.Serializable]
public class DecisionTreeAction : DecisionTreeNode
{
	public ProbAction [] actions; 

	public void Normalize()
	{
		float sum_prob = 0f; 
		foreach(ProbAction p in actions)
		{
			sum_prob += p.prob; 
		}

		float cum_sum = 0f; 
		for (int i = 0; i<actions.Length; i++) 
		{
			actions[i].prob /= sum_prob; 
			actions[i].prob += cum_sum;
			cum_sum = actions[i].prob; 
		}
	}

	public DecisionTreeNode Decide(GSR s)
	{
		return this;
		// return null; 
	}

	public string Sample()
	{
		float selector = Random.Range(0f,1f); 
		foreach(ProbAction p in actions)
		{
			if(selector<p.prob)
				return p.decision_id; 
		}

		return "Error"; 
	}

	public string ToString()
	{	
		string recap = "Action node with "; 
		foreach(ProbAction pa in actions) 
		{
			recap += "\n" + pa.ToString(); 
		}
		recap += "\n Length: " + actions.Length.ToString(); 
		return recap; 
	}
}

[System.Serializable]
public struct ProbAction
{
	public string decision_id;
	public float prob;

	public string ToString()
	{
		return "ProbAction of " + decision_id + " is " + prob.ToString(); 
	}
}

[System.Serializable]
public struct GSR
{
	public float [] state; 
}


// public class DecisionTreeAction(DecisionTreeNode)
// {
// 	public ProbAction [] actions; 

// 	public void Normalize()
// 	{
// 		float sum_prob = 0f; 
// 		foreach(ProbAction p in actions)
// 		{
// 			sum_prob += p.prob; 
// 		}
// 		float cum_sum = 0f; 
// 		foreach(ProbAction p in actions)
// 		{
// 			p.prob /= sum_prob; 
// 			p.prob += cum_sum;
// 			cum_sum = p.prob; 
// 		}
// 	}

// 	public string MakeDecision()
// 	{
// 		float selector = Random.Range(0f,1f); 
// 		foreach(ProbAction p in actions)
// 		{
// 			if(selector<p.prob)
// 				return p.decision_id; 
// 		}

// 		return "Error"; 
// 	}
// }





