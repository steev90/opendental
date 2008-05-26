#region MigraDoc - Creating Documents on the Fly
//
// Authors:
//   David Stephensen (mailto:David.Stephensen@pdfsharp.com)
//
// Copyright (c) 2001-2007 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://www.migradoc.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using PdfSharp.Charting;

namespace MigraDoc.Rendering.ChartMapper
{
  internal class DataLabelMapper
  {
    private DataLabelMapper()
    {
    }

    void MapObject(DataLabel dataLabel, MigraDoc.DocumentObjectModel.Shapes.Charts.DataLabel domDataLabel)
    {
      if (!domDataLabel.IsNull("Style"))
        FontMapper.Map(dataLabel.Font, domDataLabel.Document, domDataLabel.Style);
      if (!domDataLabel.IsNull("Font"))
        FontMapper.Map(dataLabel.Font, domDataLabel.Font);
      dataLabel.Format = domDataLabel.Format;
      if (!domDataLabel.IsNull("Position"))
        dataLabel.Position = (DataLabelPosition)domDataLabel.Position;
      if (!domDataLabel.IsNull("Type"))
        dataLabel.Type = (DataLabelType)domDataLabel.Type;
    }

    internal static void Map(DataLabel dataLabel, MigraDoc.DocumentObjectModel.Shapes.Charts.DataLabel domDataLabel)
    {
      DataLabelMapper mapper = new DataLabelMapper();
      mapper.MapObject(dataLabel, domDataLabel);
    }
  }
}
