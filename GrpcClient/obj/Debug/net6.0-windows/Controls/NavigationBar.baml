   M S B A M L   `   `   `  ����E  AGrpcClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:GrpcClient.ConvertersGrpcClient.Converters  5      9!clr-namespace:GrpcClient.ControlsGrpcClient.Controls  5      I)clr-namespace:GrpcClient.State.NavigatorsGrpcClient.State.Navigators  5	      =#clr-namespace:GrpcClient.ViewModelsGrpcClient.ViewModels  5
      Q-clr-namespace:GrpcClient.State.AuthenticatorsGrpcClient.State.Authenticators  5      '    !GrpcClient.Controls.NavigationBar   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      :
converters#clr-namespace:GrpcClient.Converters     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:GrpcClient.Controls     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      9nav)clr-namespace:GrpcClient.State.Navigators     5	      2vm#clr-namespace:GrpcClient.ViewModels     5
      >auth-clr-namespace:GrpcClient.State.Authenticators     5        � 	Resources  5      :   4GrpcClient.Converters.EqualValueToParameterConverter "  EqualValueToParameterConverter �� 
TargetType  logOutLinkStyle  NavLink �� BasedOn �� Property �� Value �� Triggers �� Binding �� Value��%  &	        &	      7    5   
   64   �� 5   
   " ��6   8   �5      �� 5       � ��$ Right=�68   6H    5      �� 5       6   �� *
IsLoggedIn+ True6=   .a�5      ��  � y�$ Visible=�63   5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5"   
   � $��0��6     BrushPrimary1#��[ 6   .+�5#      �� 	 �� Padding$	 10q�6   $
��1000��5$      $��28(�5%      
�������$��MovieFlx��5'      5(      � 5*   
   $��1��6     BrushPrimary2#��[ 5+      
 �� EdgeMode$
 Aliased=�5,      .+�5-      �   5.        	NavButton��%V   '	�       7  '��      7  �� 5/      8   " 	�6A   �� 50      8   " ��6?   51       � ColumnDefinitions 52      �� 53      $
��auto �6'   �� 54      $
��auto �6'   �� 55      $
��auto �6'   �� 56      $
��auto �6'   �� 57      $
��auto �6'   �� 58      $��* �6'   .+�5;      	� $��0��6   $   GrpcClient.ViewModels.ViewType   Home#��Z 5=      $��Trang chủ��5>      ��5<      �� *UpdateCurrentViewModelCommand+ d� 	IsChecked 5?      �� *CurrentViewModel+ �� Mode$ OneWay=� �� 	Converter# [   �� ConverterParameter)   #GrpcClient.ViewModels.HomeViewModel"  	� 5@      $��1��6      Login#��Z 5B      $��Gợi ý��5C      ��5A      �� *UpdateCurrentViewModelCommand+ 5D      �� *CurrentViewModel+$ OneWay=�# [  *   $GrpcClient.ViewModels.LoginViewModel"  �� 5E      -   5        $��	logOutLbl��5E   f   $��6��6   $��Đăng xuất��6,   #��[ 6=   5G      5H      5I      5J      5K      