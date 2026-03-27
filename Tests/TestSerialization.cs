// SPDX-License-Identifier: MIT
using Xunit;

namespace Mbemc.DataExchange.Tests;

public class TestSerialization : TestWithClearWorkingFolder
{
    #region Public Methods

    [Fact]
    public void TestWrite1()
    {
        var generatorSettings2 = new DxValueGeneratorSettings()
        {
            Start = -250,
            Length = 5e+5m,
            Step = 2e-1m,
            Mode = DxValueGeneratorMode.Linear
        };
        var measurementResultsPlug2 = new DxValueGenerator(generatorSettings2).Select(t => new Plug2()
        {
            TimeStamp = t,
            Current1 = Math.Sin(t / 25000),
            Current2 = Math.Abs(Math.Sin(t / 25000)),
            Voltage = Math.Cos(t / 50000),
        });

        var generatorSettings1 = new DxValueGeneratorSettings()
        {
            Start = -125,
            Length = 500,
            Step = 20,
            Mode = DxValueGeneratorMode.Linear
        };
        var measurementResultsPlug1 = new DxValueGenerator(generatorSettings1).Select(t => new Plug1()
        {
            TimeStamp = t,
            GRA = Math.Sin(t / 2.5)
        });

        //create writer
        var writer = new DxWriter(TestSettings);

        //add measurement 1 - plug 1 definition
        var plug1 = writer.CreateDataItem<Plug1>("Messung 1");
        //set generator or add variables
        plug1.SetGeneratorSettings(nameof(Plug1.TimeStamp), generatorSettings1);
        //write the plug 1 readings
        plug1.Write(measurementResultsPlug1);

        //add measurement 1 - plug 2 readings
        var plug2 = writer.CreateDataItem<Plug2>("Messung 1");
        //set generator or add variables
        plug2.SetGeneratorSettings(nameof(Plug2.TimeStamp), generatorSettings2);
        //write the plug 2 readings
        plug2.Write(measurementResultsPlug2);

        //write the file
        writer.Save();
    }

    [Fact]
    public void TestWrite2()
    {
        var generatorSettings2 = new DxValueGeneratorSettings()
        {
            Start = -250,
            Length = 5e+5m,
            Step = 2e-1m,
            Mode = DxValueGeneratorMode.Linear
        };
        var measurementResultsPlug2 = new DxValueGenerator(generatorSettings2).Select(t => new Plug2()
        {
            TimeStamp = t,
            Current1 = Math.Sin(t / 25000),
            Current2 = Math.Abs(Math.Sin(t / 25000)),
            Voltage = Math.Cos(t / 50000),
        });

        var generatorSettings1 = new DxValueGeneratorSettings()
        {
            Start = -125,
            Length = 500,
            Step = 20,
            Mode = DxValueGeneratorMode.Linear
        };
        var measurementResultsPlug1 = new DxValueGenerator(generatorSettings1).Select(t => new Plug1()
        {
            TimeStamp = t,
            GRA = Math.Sin(t / 2.5)
        });

        //create writer with unique filenames
        var writer = new DxWriter(TestSettings);

        //add measurement 1 - plug 1 definition
        var plug1 = writer.CreateDataItem<Plug1>("Messung 2");
        //set generator or add variables
        plug1.SetGeneratorSettings(nameof(Plug1.TimeStamp), generatorSettings1);
        //write the plug 1 readings
        plug1.Write(measurementResultsPlug1);

        //add measurement 1 - plug 2 readings
        var plug2 = writer.CreateDataItem<Plug2>("Messung 2");
        //set generator or add variables
        plug2.SetGeneratorSettings(nameof(Plug2.TimeStamp), generatorSettings2);
        //write the plug 2 readings
        plug2.Write(measurementResultsPlug2);

        //write the file
        writer.Save();
    }

    [Fact]
    public void TestGeneratorSteppingLinear()
    {
        var generatorSettings = new DxValueGeneratorSettings()
        {
            Start = -125,
            Length = 14,
            Step = 20,
            Mode = DxValueGeneratorMode.Linear
        };
        var generator = new DxValueGenerator(generatorSettings);

        double[] expected = { -125, -105, -85, -65, -45, -25, -5, 15, 35, 55, 75, 95, 115, 135 };

        var i = 0;
        foreach (var value in generator)
        {
            Assert.Equal(expected[i++], value);
        }
        Assert.Equal(i, generatorSettings.ValueCount);
    }

    [Fact]
    public void TestGeneratorSteppingLogarithmic()
    {
        var generatorSettings = new DxValueGeneratorSettings()
        {
            Start = 1,
            Length = 5,
            Step = 1,
            Mode = DxValueGeneratorMode.Logarithmic
        };
        var generator = new DxValueGenerator(generatorSettings);

        double[] expected = { 1, 1.01, 1.0201, 1.030301, 1.04060401, 1.05101005 };

        var i = 0;
        foreach (var value in generator)
        {
            Assert.Equal(expected[i++], value);
        }
        Assert.Equal(i, generatorSettings.ValueCount);
    }

    #endregion Public Methods
}
