using System;
using System.IO;
using System.Linq;

class MainClass {
  public static void Main (string[] args) {
    neuron [] inputLayer = new neuron[10];
    neuron [] outputLayer = new neuron[10];
    string trainingPath = "training.txt";
    string[] trainingData = File.ReadAllLines(trainingPath);
    float []actualOutput;
    float []expectedOutput;

    for (int i = 0; i < inputLayer.Length; i++)
    {
      inputLayer[i] = new neuron();
      inputLayer[i].initialize(1);
    }
    for (int i = 0; i < outputLayer.Length; i++)
    {
      outputLayer[i] = new neuron();
      outputLayer[i].initialize(inputLayer.Length);
    }
    
    for (int i = 0; i < 10000; i++)
    {
      for (int j = 0; j < trainingData.Length; j++)
      {
        string[]currLine = trainingData[j].Split(' ');
        for (int a = 0; a < inputLayer.Length; a++)
        {
          inputLayer[a].inputs[0] = 0.0f;
        }
      
        inputLayer[int.Parse(currLine[0])].inputs[0] = 1.0f;
        inputLayer[int.Parse(currLine[1])+5].inputs[0] = 1.0f;
        
        for (int a = 0; a < inputLayer.Length; a++)
        {
          inputLayer[a].calculate();
        }
        
        for (int a = 0; a < outputLayer.Length; a++)
        {
          for (int b = 0; b < inputLayer.Length; b++)
          {
            outputLayer[a].inputs[b] = inputLayer[b].output;
          }
        }
        
        for (int a = 0; a < outputLayer.Length; a++)
        {
          outputLayer[a].calculate();
        }
        
        actualOutput = new float[outputLayer.Length];
        for (int a = 0; a < outputLayer.Length; a++)
        {
          actualOutput[a] = outputLayer[a].output;
        }
        expectedOutput = new float[outputLayer.Length];
        expectedOutput[int.Parse(currLine[2])] = 1.0f;
        for (int a = 0; a < outputLayer.Length; a++)
        {
          outputLayer[a].adjust(expectedOutput[a],actualOutput[a]);
        }
        
      }
    }
    
    while(true)
    {
      Console.Write("Enter 2 numbers that are greater than or equal to 1 and less than or equal to 5: ");
      string[] input = Console.ReadLine().Split(' ');
      for (int a = 0; a < inputLayer.Length; a++)
      {
        inputLayer[a].inputs[0] = 0.0f;
      }
      inputLayer[int.Parse(input[0])].inputs[0] = 1.0f;
      inputLayer[int.Parse(input[1])+5].inputs[0] = 1.0f;
      for (int a = 0; a < inputLayer.Length; a++)
      {
        inputLayer[a].calculate();
      }
      for (int a = 0; a < outputLayer.Length; a++)
      {
        for (int b = 0; b < inputLayer.Length; b++)
        {
          outputLayer[a].inputs[b] = inputLayer[b].output;
        }
      }
      for (int a = 0; a < outputLayer.Length; a++)
      {
        outputLayer[a].calculate();
      }
      actualOutput = new float[outputLayer.Length];
      for (int i = 0; i < outputLayer.Length; i++)
      {
        actualOutput[i] = outputLayer[i].output;
      } 
      Console.WriteLine(actualOutput.ToList().IndexOf(actualOutput.Max()));
    }
  }
}
class neuron
{
  Random randnum = new Random();
  public float[] inputs;
  public float[] weights;
  public float[] weightAdjustments;        
  public float output;
  public float bias;
  static float[] signModifier = {-1.0f,1.0f};

  public void initialize(int numOfInputs)
  {
    inputs = new float[numOfInputs];
    weights = new float[numOfInputs];
    bias = 10.0f*(float)randnum.NextDouble()*signModifier[randnum.Next(0,2)];
    weightAdjustments = new float[numOfInputs];

    for (int i = 0; i < weights.Length; i++)
    {
      weights[i] = 1.0f*(float)randnum.NextDouble()*signModifier[randnum.Next(0,2)];
    }
  }
  public void calculate()
  {
    float total = 0;
    for (int i = 0; i < inputs.Length; i++)
    {
      total += inputs[i] * weights[i];
    }
    total += bias;
    output = sigmoid(total);
  }
  public void adjust(float actualOutput,float calculatedOutput)
  {
    float factor = 0.0f;
    for (int i = 0; i < weights.Length; i++)
    {
      factor = (actualOutput-calculatedOutput)*calculatedOutput*(1-calculatedOutput);
      weights[i] += factor*inputs[i];
    }
    bias += factor;
  }
  public float sigmoid (float input)
  {
    return (float)(1/(1+Math.Exp((double)-input)));
  }
}
