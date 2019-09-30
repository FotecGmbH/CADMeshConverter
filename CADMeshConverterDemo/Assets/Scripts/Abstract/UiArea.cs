// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        13.09.2019 07:10
// Developer:      Georg Wernitznig (GWe)
// Project:        CADMeshConverterDemo
//
// Released under GPL-3.0-only


using System;
using UnityEngine;

namespace Assets.Scripts.Abstract
{
    /// <summary>
    /// <para>Abstract class for UI areas</para>
    /// Klasse IUiArea. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public abstract class UiArea : MonoBehaviour
    {
        /// <summary>
        /// An event that gets fired when the <see cref="UiArea"/> should be closed.
        /// </summary>
        public abstract event EventHandler CloseArea;

        /// <summary>
        /// An object containing a result of the area.
        /// </summary>
        public abstract object AreaResult { get; set; }
    }
}