using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Javax.Microedition.Khronos.Egl;
using Javax.Microedition.Khronos.Opengles;
using static Android.Opengl.GLSurfaceView;  // 类型之前需要加上static关键字,命名空间之前不用加
using Java.Nio;

namespace OpenGLRotatePolygon
{
    /// <summary>
    /// 旋转多边形的OpenGL测试代码
    /// 实现自定义的渲染程序
    /// 继承Java.Lang.Object可以不用事先Dispose()方法和Handle属性
    /// </summary>
    public class MyRenderer : Java.Lang.Object, IRenderer
    {

        float[] triangleData = new float[]
{
            0.1f,0.6f,0.0f,     // 上顶点
            -0.3f,0.0f,0.0f,    // 左顶点
            0.3f,0.1f,0.0f      // 右顶点
};

        int[] triangleColor = new int[]
        {
            65535,0,0,0,    // 上顶点红色
            0,65535,0,0,    // 左顶点绿色
            0,0,65535,0     // 右顶点蓝色
        };

        float[] rectData = new float[]
        {
            0.4f,0.4f,0.0f,     // 右上顶点
            0.4f,-0.4f,0.0f,    // 右下顶点
            -0.4f,0.4f,0.0f,    // 左上顶点
            -0.4f,-0.4f,0.0f    // 左下顶点
        };

        int[] rectColor = new int[]
        {
            0,65535,0,0,        // 右上顶点绿色
            0,0,65535,0,        // 右下顶点蓝色
            65535,0,0,0,        // 左上顶点红色
            65535,65535,0,0     // 左下顶点黄色
        };

        /// <summary>
        /// 依然是正方形的四个顶点,只是顺序交换了一下
        /// </summary>
        float[] rectData2 = new float[]
        {
            -0.4f,0.4f,0.0f,    // 左上顶点
            0.4f,0.4f,0.0f,     // 右上顶点
            0.4f,-0.4f,0.0f,    // 右下顶点
            -0.4f,-0.4f,0.0f    // 左下顶点
        };

        /// <summary>
        /// 五角星形
        /// </summary>
        float[] pentacle = new float[]
        {
            0.4f,0.4f,0.0f,
            -0.2f,0.3f,0.0f,
            0.5f,0.0f,0f,
            -0.4f,0.0f,0f,
            -0.1f,-0.3f,0f
        };

        FloatBuffer triangleDataBuffer;
        IntBuffer triangleColorBuffer;
        FloatBuffer rectDataBuffer;
        IntBuffer rectColorBuffer;
        FloatBuffer rectDataBuffer2;
        FloatBuffer pentacleBuffer;
        /// <summary>
        /// 控制旋转的角度
        /// </summary>
        private float rotate;

        /// <summary>
        /// 重点,注释掉的Wrap()方法会导致 called a GL11 Pointer method with an indirect Buffer 错误
        /// 使用bufferUtil()方法可以修复这个问题
        /// </summary>
        public MyRenderer()
        {
            // 将顶点位置数组包装成FloatBuffer;
            /*
            triangleDataBuffer = FloatBuffer.Wrap(triangleData);
            rectDataBuffer = FloatBuffer.Wrap(rectData);
            rectDataBuffer2 = FloatBuffer.Wrap(rectData2);
            pentacleBuffer = FloatBuffer.Wrap(pentacle);
            */
            triangleDataBuffer = bufferUtil(triangleData);
            rectDataBuffer = bufferUtil(rectData);
            rectDataBuffer2 = bufferUtil(rectData2);
            pentacleBuffer = bufferUtil(pentacle);
            // 将顶点颜色数据数组包装成IntBuffer
            /*
            triangleColorBuffer = IntBuffer.Wrap(triangleColor);
            rectColorBuffer = IntBuffer.Wrap(rectColor);
            */
            triangleColorBuffer = bufferUtil(triangleColor);
            rectColorBuffer = bufferUtil(rectColor);
        }

        public void OnDrawFrame(IGL10 gl)
        {
            // 清除屏幕缓存和深度缓存
            gl.GlClear(GL10.GlColorBufferBit | GL10.GlDepthBufferBit);
            // 启用顶点坐标数据
            gl.GlEnableClientState(GL10.GlVertexArray);
            // 启用顶点颜色数据
            gl.GlEnableClientState(GL10.GlColorArray);
            // 设置当前矩阵堆栈为模型堆栈
            gl.GlMatrixMode(GL10.GlModelview);
            // --------------------绘制第一个图形---------------------
            // 重置当前的模型视图矩阵
            gl.GlLoadIdentity();
            gl.GlTranslatef(-0.32f, 0.35f, -1.2f);
            // 设置顶点的位置数据
            gl.GlVertexPointer(3, GL10.GlFloat, 0, triangleDataBuffer);
            // 设置顶点的颜色数据
            gl.GlColorPointer(4, GL10.GlFixed, 0, triangleColorBuffer);

            // 根据顶点数据绘制平面图形
            gl.GlDrawArrays(GL10.GlTriangles, 0, 3);
            // --------------------绘制第二个图形---------------------
            // 重置当前的模型视图矩阵
            gl.GlLoadIdentity();
            gl.GlTranslatef(0.6f, 0.8f, -1.5f);
            gl.GlRotatef(rotate, 0f, 0f, 0.1f);
            // 设置顶点的位置数据
            gl.GlVertexPointer(3, GL10.GlFloat, 0, rectDataBuffer);
            // 设置顶点的颜色数据
            gl.GlColorPointer(4, GL10.GlFixed, 0, rectColorBuffer);
            // 根据顶点数据绘制平面图形
            gl.GlDrawArrays(GL10.GlTriangleStrip, 0, 4);
            // --------------------绘制第三个图形---------------------
            // 重置当前的模型视图矩阵
            gl.GlLoadIdentity();
            gl.GlTranslatef(-0.4f, -0.5f, -1.5f);
            gl.GlRotatef(rotate, 0f, 0.2f, 0f);
            // 设置顶点的位置数据（依然使用之前的顶点颜色）
            gl.GlVertexPointer(3, GL10.GlFloat, 0, rectDataBuffer2);
            // 根据顶点数据绘制平面图形
            gl.GlDrawArrays(GL10.GlTriangleStrip, 0, 4);
            // --------------------绘制第四个图形---------------------
            // 重置当前的模型视图矩阵
            gl.GlLoadIdentity();
            gl.GlTranslatef(0.4f, -0.5f, -1.5f);
            // 设置使用纯色填充
            gl.GlColor4f(1.0f, 0.2f, 0.2f, 0.0f);
            gl.GlDisableClientState(GL10.GlColorArray);
            // 设置顶点的位置数据
            gl.GlVertexPointer(3, GL10.GlFloat, 0, pentacleBuffer);
            // 根据顶点数据绘制平面图形
            gl.GlDrawArrays(GL10.GlTriangleStrip, 0, 5);
            // 绘制结束
            gl.GlFinish();
            gl.GlDisableClientState(GL10.GlVertexArray);
            // 旋转角度增加1
            rotate += 1;
        }

        public void OnSurfaceChanged(IGL10 gl, int width, int height)
        {
            // 设置3D视窗的大小及位置
            gl.GlViewport(0, 0, width, height);
            // 将当前矩阵模式设为投影矩阵
            gl.GlMatrixMode(GL10.GlProjection);
            // 初始化单位矩阵
            gl.GlLoadIdentity();
            // 计算透视视窗的宽度、高度比
            float ratio = (float)width / height;
            // 调用此方法设置透视视窗的空间大小
            gl.GlFrustumf(-ratio, ratio, -1, 1, 1, 10);
        }

        public void OnSurfaceCreated(IGL10 gl, EGLConfig config)
        {
            // 关闭抗抖动
            gl.GlDisable(GL10.GlDither);
            // 设置系统对透视进行修正
            gl.GlHint(GL10.GlPerspectiveCorrectionHint, GL10.GlFastest);
            gl.GlClearColor(0, 0, 0, 0);
            // 设置阴影平滑模式
            gl.GlShadeModel(GL10.GlSmooth);
            // 启用深度测试
            gl.GlEnable(GL10.GlDepthTest);
            // 设置深度测试的类型
            gl.GlDepthFunc(GL10.GlLequal);
        }


        public IntBuffer bufferUtil(int[] arr)
        {
            IntBuffer buffer;

            ByteBuffer qbb = ByteBuffer.AllocateDirect(arr.Length * 4);
            qbb.Order(ByteOrder.NativeOrder());

            buffer = qbb.AsIntBuffer();
            buffer.Put(arr);
            buffer.Position(0);

            return buffer;
        }

        public FloatBuffer bufferUtil(float[] arr)
        {
            FloatBuffer buffer;

            ByteBuffer qbb = ByteBuffer.AllocateDirect(arr.Length * 4);
            qbb.Order(ByteOrder.NativeOrder());

            buffer = qbb.AsFloatBuffer();
            buffer.Put(arr);
            buffer.Position(0);

            return buffer;
        }

    }
}