using System.Collections;
using System.Collections.Generic;

public class Tensor<T>
{
    /*
    not thread safe
    */

    T[] data;
    int[] dims;
    int[] strides;
    int startOffset;

    public Tensor(int[] dims) {
        this.dims = (int[])dims.Clone();
        int size = Size();
        this.data = new T[size];
        int _stride = 1;
        this.strides = new int[dims.Length];
        for(int d = dims.Length - 1; d >= 0; d--) {
            strides[d] = _stride;
            _stride *= dims[d];
        }
        startOffset = 0;
    }

    public Tensor(int d0) {
        int[] dims = new int[]{d0};
        this.dims = (int[])dims.Clone();
        int size = Size();
        this.data = new T[size];
        int _stride = 1;
        this.strides = new int[dims.Length];
        for(int d = dims.Length - 1; d >= 0; d--) {
            strides[d] = _stride;
            _stride *= dims[d];
        }
        startOffset = 0;
    }

    public Tensor(int d0, int d1) {
        int[] dims = new int[]{d0, d1};
        this.dims = (int[])dims.Clone();
        int size = Size();
        this.data = new T[size];
        int _stride = 1;
        this.strides = new int[dims.Length];
        for(int d = dims.Length - 1; d >= 0; d--) {
            strides[d] = _stride;
            _stride *= dims[d];
        }
        startOffset = 0;
    }

    public Tensor(int d0, int d1, int d2) {
        int[] dims = new int[]{d0, d1, d2};
        this.dims = (int[])dims.Clone();
        int size = Size();
        this.data = new T[size];
        int _stride = 1;
        this.strides = new int[dims.Length];
        for(int d = dims.Length - 1; d >= 0; d--) {
            strides[d] = _stride;
            _stride *= dims[d];
        }
        startOffset = 0;
    }

    public Tensor(int startOffset, int[] dims, int[] strides, T[] data) {
        // does NOT copy the data
        this.startOffset = startOffset;
        this.dims = (int[])dims.Clone();
        this.strides = (int[])strides.Clone();
        this.data = data;
    }

    public int Size() {
        int size = 1;
        foreach(int d in dims) {
            size *= d;
        }
        return size;
    }

    public override string ToString() {
        string res = "Tensor<" + typeof(T).Name + ">(";
        res += string.Join(", ", dims);
        res += ")";
        if(dims.Length == 1) {
            res = "{";
            for(int x = 0; x < dims[0]; x++) {
                res += data[Offset(new int[]{x})];
                res += ", ";
            }
            res += "},\n";
        } else if( dims.Length == 2) {
            res += "{";
            for(int y = 0; y < dims[0]; y++) {
                res += "{";
                for(int x = 0; x < dims[1]; x++) {
                    res += data[Offset(new int[]{y, x})];
                    res += ", ";
                }
                res += "},\n";
            }
            res += "}";
        }
        return res;
    }

    int Offset(int[] idx) {
        int offset = startOffset;
        for(int d=0; d < dims.Length; d++) {
            offset += idx[d] * strides[d];
        }
        return offset;
    }

    public T this[int[] idx] {
        get {
            return data[Offset(idx)];
        }
        set {
            data[Offset(idx)] = value;
        }
    }

    public T this[int idx] {
        get {
            return data[Offset(new int[]{idx})];
        }
        set {
            data[Offset(new int[]{idx})] = value;
        }
    }

    public T this[int d0, int d1] {
        get {
            return data[Offset(new int[]{d0, d1})];
        }
        set {
            data[Offset(new int[]{d0, d1})] = value;
        }
    }

    public int[] Shape() {
        return (int[])dims.Clone();
    }

    public Tensor<T> Transpose(int[] newDimIdxes) {
        int[] newStrides = (int[])strides.Clone();
        int[] newDims = (int[])dims.Clone();
        for(int d = 0; d < dims.Length; d++) {
            newStrides[d] = strides[newDimIdxes[d]];
            newDims[d] = dims[newDimIdxes[d]];
        }

        return new Tensor<T>(0, newDims, newStrides, data);
    }

    public Tensor<T> Slice(int d, int start, int end) {
        int newOffset = startOffset + start * strides[d];
        int[] newStrides = (int[])strides.Clone();
        int[] newDims = (int[])dims.Clone();
        newDims[d] = end - start;
        return new Tensor<T>(newOffset, newDims, newStrides, data);
    }

    public Tensor<T> Select(int d, int idx) {
        int newOffset = startOffset + idx * strides[d];
        int[] newStrides = new int[dims.Length - 1];
        int[] newDims = new int[dims.Length - 1];
        for(int _d=0; _d < dims.Length; _d++) {
            if(_d < d) {
                newStrides[_d] = strides[_d];
                newDims[_d] = dims[_d];
            } else if(_d > d) {
                newStrides[_d - 1] = strides[_d];
                newDims[_d - 1] = dims[_d];
            }
        }
        return new Tensor<T>(newOffset, newDims, newStrides, data);
    }
}
