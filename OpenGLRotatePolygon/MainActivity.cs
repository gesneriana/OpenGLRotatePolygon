using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Opengl;

namespace OpenGLRotatePolygon
{
    [Activity(Label = "OpenGLRotatePolygon", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // 创建一个GLSurfaceView，用于显示OpenGL绘制的图形
            GLSurfaceView glView = new GLSurfaceView(this);
            // 创建GLSurfaceView的内容绘制器
            MyRenderer myRender = new MyRenderer();
            // 为GLSurfaceView设置绘制器
            glView.SetRenderer(myRender);
            SetContentView(glView);
        }
    }
}