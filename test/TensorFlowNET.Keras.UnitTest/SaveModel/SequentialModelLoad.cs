﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Tensorflow;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Optimizers;
using Tensorflow.Keras.UnitTest.Helpers;
using Tensorflow.NumPy;
using static Tensorflow.Binding;
using static Tensorflow.KerasApi;

namespace TensorFlowNET.Keras.UnitTest.SaveModel;

[TestClass]
public class SequentialModelLoad
{
    [TestMethod]
    public void SimpleModelFromAutoCompile()
    {
        var model = tf.keras.models.load_model(@"Assets/simple_model_from_auto_compile");
        model.summary();

        model.compile(new Adam(0.0001f), tf.keras.losses.SparseCategoricalCrossentropy(), new string[] { "accuracy" });

        // check the weights
        var kernel1 = np.load(@"Assets/simple_model_from_auto_compile/kernel1.npy");
        var bias0 = np.load(@"Assets/simple_model_from_auto_compile/bias0.npy");

        Assert.IsTrue(kernel1.Zip(model.TrainableWeights[2].numpy()).All(x => x.First == x.Second));
        Assert.IsTrue(bias0.Zip(model.TrainableWeights[1].numpy()).All(x => x.First == x.Second));

        var data_loader = new MnistModelLoader();
        var num_epochs = 1;
        var batch_size = 8;

        var dataset = data_loader.LoadAsync(new ModelLoadSetting
        {
            TrainDir = "mnist",
            OneHot = false,
            ValidationSize = 58000,
        }).Result;

        model.fit(dataset.Train.Data, dataset.Train.Labels, batch_size, num_epochs);
    }

    [TestMethod]
    public void AlexnetFromSequential()
    {
        new SequentialModelSave().AlexnetFromSequential();
        var model = tf.keras.models.load_model(@"./alexnet_from_sequential");
        model.summary();

        model.compile(new Adam(0.001f), tf.keras.losses.SparseCategoricalCrossentropy(from_logits: true), new string[] { "accuracy" });

        var num_epochs = 1;
        var batch_size = 8;

        var dataset = new RandomDataSet(new Shape(227, 227, 3), 16);

        model.fit(dataset.Data, dataset.Labels, batch_size, num_epochs);
    }

    [TestMethod]
    public void ModelWithSelfDefinedModule()
    {
        var model = tf.keras.models.load_model(@"Assets/python_func_model");
        model.summary();

        model.compile(tf.keras.optimizers.Adam(), tf.keras.losses.SparseCategoricalCrossentropy(), new string[] { "accuracy" });

        var data_loader = new MnistModelLoader();
        var num_epochs = 1;
        var batch_size = 8;

        var dataset = data_loader.LoadAsync(new ModelLoadSetting
        {
            TrainDir = "mnist",
            OneHot = false,
            ValidationSize = 55000,
        }).Result;

        model.fit(dataset.Train.Data, dataset.Train.Labels, batch_size, num_epochs);
    }

    [Ignore]
    [TestMethod]
    public void VGG19()
    {
        var model = tf.keras.models.load_model(@"D:\development\tf.net\models\VGG19");
        model.summary();

        var classify_model = keras.Sequential(new System.Collections.Generic.List<Tensorflow.Keras.ILayer>()
        {
            model,
            keras.layers.Flatten(),
            keras.layers.Dense(10),
        });
        classify_model.summary();

        classify_model.compile(tf.keras.optimizers.Adam(), tf.keras.losses.SparseCategoricalCrossentropy(), new string[] { "accuracy" });

        var x = np.random.uniform(0, 1, (8, 512, 512, 3));
        var y = np.ones((8));

        classify_model.fit(x, y, batch_size: 4);
    }

    [Ignore]
    [TestMethod]
    public void TestModelBeforeTF2_5()
    {
        var a = keras.layers;
        var model = tf.saved_model.load(@"D:\development\temp\saved_model") as Model;
        model.summary();
    }
}
