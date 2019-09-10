﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tensorflow;
using Tensorflow.Models.ObjectDetection;
using static Tensorflow.Binding;

namespace TensorFlowNET.Examples.ImageProcessing.ObjectDetection
{
    public class Main : IExample
    {
        public bool Enabled { get; set; } = true;
        public bool IsImportingGraph { get; set; } = true;

        public string Name => "Object Detection API";

        dynamic FLAGS = new
        {
            model_dir = "D:/Projects/PythonLab/tf-models/research/object_detection/models/model"
        };

        ModelLib model_lib = new ModelLib();

        public bool Run()
        {
            var config = tf.estimator.RunConfig(model_dir: FLAGS.model_dir);
            model_lib.create_estimator_and_inputs(run_config: config);

            // Currently only a single Eval Spec is allowed.
            tf.estimator.train_and_evaluate();

            return true;
        }

        public Graph BuildGraph()
        {
            throw new NotImplementedException();
        }

        public Graph ImportGraph()
        {
            throw new NotImplementedException();
        }

        public void Predict(Session sess)
        {
            throw new NotImplementedException();
        }

        public void PrepareData()
        {
            throw new NotImplementedException();
        }



        public void Train(Session sess)
        {
            throw new NotImplementedException();
        }

        void IExample.Test(Session sess)
        {
            throw new NotImplementedException();
        }
    }
}
