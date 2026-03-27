// SPDX-License-Identifier: MIT
using Xunit;

namespace Mbemc.DataExchange.Tests;

public class TestDxValueGenerator
{
    #region Public Methods

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
            Assert.Equal(expected[i++], value, 1e-7);
        }
        Assert.Equal(i, generatorSettings.ValueCount);
    }

    [Theory]
    [InlineData(-100.0, DxValueGeneratorMode.Linear)]
    [InlineData(0.0, DxValueGeneratorMode.Linear)]
    [InlineData(+4567.0, DxValueGeneratorMode.Linear)]
    [InlineData(-100.0, DxValueGeneratorMode.Logarithmic)]
    [InlineData(0.0, DxValueGeneratorMode.Logarithmic)]
    [InlineData(+4567.0, DxValueGeneratorMode.Logarithmic)]
    public void TestGeneratorSingleStep(double start, DxValueGeneratorMode mode)
    {
        var generatorSettings = new DxValueGeneratorSettings()
        {
            Start = (decimal)start,
            Length = 1,
            Step = 123,
            Mode = mode
        };
        var generator = new DxValueGenerator(generatorSettings);

        var last = start - 1.0;
        foreach (var value in generator)
        {
            last = value;
        }
        Assert.Equal(start, last);
        Assert.Equal(1, generatorSettings.ValueCount);
    }

    [Theory]
    [InlineData(-100.0, 11, 1.0, DxValueGeneratorMode.Linear, -90.0)]
    [InlineData(+50.0, 11, 4.0, DxValueGeneratorMode.Linear, 90.0)]
    [InlineData(+50.0, 11, -2.0, DxValueGeneratorMode.Linear, 30.0)]
    [InlineData(0.0, 1000, 0.001, DxValueGeneratorMode.Linear, 0.999)]
    [InlineData(0.0, 50000, 0, DxValueGeneratorMode.Linear, 0.0)]
    [InlineData(0.0, 50000, 0, DxValueGeneratorMode.Logarithmic, 0.0)]
    [InlineData(1.0, 51, 1, DxValueGeneratorMode.Logarithmic, 1.644631822)]
    public void TestGeneratorLastValue(decimal start, decimal length, decimal step, DxValueGeneratorMode mode, double expected)
    {
        var generatorSettings = new DxValueGeneratorSettings()
        {
            Start = start,
            Length = length,
            Step = step,
            Mode = mode
        };
        var generator = new DxValueGenerator(generatorSettings);

        var last = (double)start - 1.0;
        foreach (var value in generator)
        {
            last = value;
        }
        Assert.Equal(expected, last, 1e-7);
        Assert.Equal((long)length, generatorSettings.ValueCount);
    }

    #endregion Public Methods
}
