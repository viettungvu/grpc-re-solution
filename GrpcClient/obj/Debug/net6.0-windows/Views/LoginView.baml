   M S B A M L   `   `   `  ����J  FPasswordBoxMVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`Aclr-namespace:PasswordBoxMVVM.Components;assembly=PasswordBoxMVVMPasswordBoxMVVM.Components  5      E AGrpcClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:GrpcClient.ViewsGrpcClient.Views 5          GrpcClient.Views.LoginView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      X
componentsAclr-namespace:PasswordBoxMVVM.Components;assembly=PasswordBoxMVVM     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:GrpcClient.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      $��24(�5        � 	Resources  5          BooleanToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      �� $��25q�6    �� Padding$ 50q�6   $��Center=�5   	   $��Center=�5   	   
�������  BrushPrimary2#��[ 5   	   $��3q�5   	    �� CornerRadius$ 10��5   	    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5          
BlurRadius$ 10Q�6      Color$
 Gray��6/   ._�5"   
   �  � IsSharedSizeScope$
 True��6    � RowDefinitions 5#      �� 5$      $
��auto �6    �� 5%      $
��auto �6    �� 5&      $
��auto �6    .+�5)      �� $��0��6   $��Center=�5*      $��48(�5+      $��Login MovieFlx��5,      �� 5.      $��1��6   $
��0 10q�6%   $��Center=�5/      $��18(�50      $
��Thin"�51      $*��$The best security movies application��52      � 54      $��2��6   $
��0 20q�6     55      �� 56      $
��auto �6$   �� 57      $
��auto �6$   �� 58      $
��auto �6$   �� 59      $
��auto �6$   �� 5:      $
��auto �6$   .+�5=      � $��0��6   $
��0 10q�6$    � ColumnDefinitions 5>      �� 5?      $
��auto �6+   	 z� SharedSizeGroup$	 Label��68   �� 5@      $	��300 �6+   .+�5C      �� $��0��6    $��Username��60   �� 5D      $��1��6   $��20 0 0 0q�6.   ��5E      �� *
Username+
 �� UpdateSourceTrigger$
 PropertyChanged=�5F      � 5H      $��1��6   $
��0 10q�6$    5I      �� 5J      $
��auto �6+   $	 Label��68   �� 5K      $	��300 �6+   .+�5N      �� $��0��6    $��Password��60   4   .PasswordBoxMVVM.Components.BindablePasswordBox  5O      $��1��65   $��20 0 0 0q�6E      Password 5P      �� *
Password+5Q      �� 5S      $��2��6   $
��0 10q�6&   $��Center=�5T      $��Login��5V       �� 	IsDefault$
 True��5W      ��5U      �� *LoginCommand+5W      �� 5Y      $��3��6   $��0 10 0 0q�6&   $��Center=�5Z      
������ $��Don't have an account?��5]      $��14(�5^      
����  ���5\      �� *ViewRegisterCommand+5_      �� 5a      $��4��6   $��0 10 0 0q�6)   $��Center=�5b      
���  �� � DataContext 5c      �� *ErrorMessageViewModel+��5e      �� *	Message+y�5f      �� *
HasMessage+ �� 	Converter# [  5g      5h      5i      5j      