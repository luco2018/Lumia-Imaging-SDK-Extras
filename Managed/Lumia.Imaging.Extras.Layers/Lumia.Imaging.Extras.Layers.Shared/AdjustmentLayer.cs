﻿using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Lumia.Imaging.Extras.Layers
{
    /// <summary>
    /// Represents an adjustment layer containing an effect or a list of filters that will be applied to the image.
    /// </summary>
    public sealed class AdjustmentLayer : Layer
    {
        private const TaskContinuationOptions ContinuationOptions =
            TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion;

        /// <summary>
        /// Creates an adjustment layer containing the specified effect.
        /// </summary>
        /// <param name="layerStyle">Layer style to use for this layer.</param>
        /// <param name="effect">The effect to apply in the adjustment layer.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, IImageProvider effect) :
            this(layerStyle, context => effect)
        {
        }

        /// <summary>
        /// Creates an adjustment layer containing the specified effect.
        /// </summary>
        /// <param name="layerStyle">Layer style to use for this layer.</param>
        /// <param name="effectTask">An already started task that asynchronously results in the effect to apply in the adjustment layer.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Task<IImageProvider> effectTask) :
            base(layerStyle, context => new MaybeTask<IImageProvider>(effectTask), Size.Empty)
        {
        }

        /// <summary>
        /// Creates an adjustment layer containing the specified effect.
        /// </summary>
        /// <param name="layerStyle">Layer style to use for this layer.</param>
        /// <param name="effectTask">An already started task that asynchronously results in the effect to apply in the adjustment layer.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, MaybeTask<IImageProvider> effectTask) :
            base(layerStyle, effectTask, Size.Empty)
        {
        }

        /// <summary>
        /// Creates an adjustment layer containing the specified effect.
        /// </summary>
        /// <param name="layerStyle">Layer style to use for this layer.</param>
        /// <param name="effectResolver">A lazy resolve func that returns the effect <see cref="IImageProvider" /> based on context sensitive values such as source image size.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Func<LayerContext, IImageProvider> effectResolver) :
            base(layerStyle, context => new MaybeTask<IImageProvider>(effectResolver(context)), Size.Empty)
        {
        }

        /// <summary>
        /// Creates an adjustment layer containing the specified effect.
        /// </summary>
        /// <param name="layerStyle">Layer style to use for this layer.</param>
        /// <param name="effectResolver">A lazy resolve func that asynchronously returns the effect <see cref="IImageProvider" /> based on context sensitive values such as source image size.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Func<LayerContext, Task<IImageProvider>> effectResolver) :
            base(layerStyle, context => new MaybeTask<IImageProvider>(effectResolver(context)), Size.Empty)
        {
        }

        /// <summary>
        /// Creates an adjustment layer containing the specified effect.
        /// </summary>
        /// <param name="layerStyle">Layer style to use for this layer.</param>
        /// <param name="effectResolver">A lazy resolve func that asynchronously returns the effect <see cref="IImageProvider" /> based on context sensitive values such as source image size.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Func<LayerContext, MaybeTask<IImageProvider>> effectResolver) :
            base(layerStyle, effectResolver, Size.Empty)
        {
        }

        /// <summary>
        /// Adds an adjustment layer, with one or more filters, blended onto the previous layer.
        /// </summary>
        /// <param name="layerStyle">Description of how to blend this layer onto the preceding one.</param>
        /// <param name="filters">One or more filters to apply in the adjustment layer.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, params IFilter[] filters) :
            this(layerStyle, context => filters)
        {
        }

        /// <summary>
        /// Adds an adjustment layer, with one or more filters, blended onto the previous layer.
        /// </summary>
        /// <param name="layerStyle">Description of how to blend this layer onto the preceding one.</param>
        /// <param name="filterTask">An already started task that results in an <see cref="IFilter" /> to apply in the adjustment layer.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Task<IFilter> filterTask) :
            this(layerStyle, context => filterTask)
        {
        }

        /// <summary>
        /// Adds an adjustment layer, with one or more filters, blended onto the previous layer.
        /// </summary>
        /// <param name="layerStyle">Description of how to blend this layer onto the preceding one.</param>
        /// <param name="filterArrayTask">An already started task that results in an array of <see cref="IFilter" /> to apply in the adjustment layer.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Task<IFilter[]> filterArrayTask) :
            this(layerStyle, context => filterArrayTask)
        {
        }

        /// <summary>
        /// Adds an adjustment layer, with one or more filters, blended onto the previous layer.
        /// </summary>
        /// <param name="layerStyle">Description of how to blend this layer onto the preceding one.</param>
        /// <param name="filterResolver">A lazy resolve func that returns an <see cref="IFilter" /> based on context sensitive values such as source image size.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Func<LayerContext,IFilter> filterResolver) :
            base(layerStyle, context => new MaybeTask<IImageProvider>(new FilterEffect { Filters = new IFilter[] { filterResolver(context) } }), Size.Empty)
        {
        }

        /// <summary>
        /// Adds an adjustment layer, with one or more filters, blended onto the previous layer.
        /// </summary>
        /// <param name="layerStyle">Description of how to blend this layer onto the preceding one.</param>
        /// <param name="filterArrayResolver">A lazy resolve func that returns an array of <see cref="IFilter" /> based on context sensitive values such as source image size.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Func<LayerContext, IFilter[]> filterArrayResolver) :
            base(layerStyle, context => new MaybeTask<IImageProvider>(new FilterEffect { Filters = filterArrayResolver(context) }), Size.Empty)
        {
        }

        /// <summary>
        /// Adds an adjustment layer, with one or more filters, blended onto the previous layer.
        /// </summary>
        /// <param name="layerStyle">Description of how to blend this layer onto the preceding one.</param>
        /// <param name="filterResolver">A lazy resolve func that returns an <see cref="IFilter" /> based on context sensitive values such as source image size.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Func<LayerContext, Task<IFilter>> filterResolver) :
            base(layerStyle, context => new MaybeTask<IImageProvider>(filterResolver(context).ContinueWith(
                (Func<Task<IFilter>, IImageProvider>)CreateFilterEffectFromFilterTask,
                ContinuationOptions)),
            Size.Empty)
        {
        }

        /// <summary>
        /// Adds an adjustment layer, with one or more filters, blended onto the previous layer.
        /// </summary>
        /// <param name="layerStyle">Description of how to blend this layer onto the preceding one.</param>
        /// <param name="filterArrayResolver">A lazy resolve func that returns an array of <see cref="IFilter" /> based on context sensitive values such as source image size.</param>
        /// <returns>The adjustment layer.</returns>
        public AdjustmentLayer(LayerStyle layerStyle, Func<LayerContext, Task<IFilter[]>> filterArrayResolver) :
            base(layerStyle, context => new MaybeTask<IImageProvider>(filterArrayResolver(context).ContinueWith(
                (Func<Task<IFilter[]>, IImageProvider>)CreateFilterEffectFromFilterArrayTask,
                ContinuationOptions)),
            Size.Empty)
        {
        }

        private static IImageProvider CreateFilterEffectFromFilterTask(Task<IFilter> filterTask)
        {
            return new FilterEffect { Filters = new[] { filterTask.Result } };
        }

        private static IImageProvider CreateFilterEffectFromFilterArrayTask(Task<IFilter[]> filterArrayTask)
        {
            return new FilterEffect { Filters = filterArrayTask.Result };
        }
    }
}
