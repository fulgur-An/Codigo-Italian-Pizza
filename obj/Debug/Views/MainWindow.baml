   M S B A M L   `   `   `  ����G  CItalianPizza, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ItalianPizzaItalianPizza  5   	       ItalianPizza.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   W SFontAwesome5.Net, Version=2.1.6.0, Culture=neutral, PublicKeyToken=9cfaf01297a008f8/fa5%http://schemas.fontawesome.com/icons/  5   	   &localclr-namespace:ItalianPizza  5   	     � Title$  
MainWindow��5
   	   $	��720��6   $
��1000��6)    � WindowStartupLocation$ CenterScreen=�66    � 
ResizeMode$ NoResize=�5   	   .��5      � ��5   
   �� 5       �� RelativeTransform 5      V� 5      .~�5      ��  �� CenterY$	 0.5Q�6)    �� CenterX$	 0.5Q�67   �� 5       �� CenterY$	 0.5Q�6(    �� CenterX$	 0.5Q�66   �� 5       �� Angle$	 120Q�6*   	 �� CenterY$		 0.5Q�66   
 �� CenterX$	
 0.5Q�6D   T� 5      5      .��5      �  � Color$ #FFFF00��6    � Offset$	 0.0Q�6/   � 5      $ #FF0000��6   $
 0.25Q�6/   � 5      $ #800080��6   $
 0.75Q�6/   � 5      $ #3630A5��6   $	 1.0Q�6/   5       .+�5&   
   � ���#372727$��210px��6$   $
��Left=�62    J� Opacity$
 0.91Q�6M   .+�5'      ��  �� Orientation$ Vertical=�6   
������ .+�5(      �� $ 
Horizontal=�6   $��
0,30px,0,0q�66   .+�5)        FontAwesome5.ImageAwesome     Icon$ Solid_PizzaSlice=�6'   $��30��6?   $��30��6K   $��25,0,0,0q�6V      
Foreground �#FFFFFF$��Center=�5*   ;   $��Center=�6V    � ToolTip$ Usuarios��5+   &   �� 5,      -   5        ���#FFFFFF$��Center=�5,   5   $��Center=�6P   $��
10px,0,0,0q�5-       $��Trebuchet MS)�64   $��20(�6N   ItalianPizza®5/      50      �� 52      $��15,35px,15,0q�6   $ Vertical=�63   .+�53      �� $��Center=�6   $	��Top=�6:    +  'MaterialDesignFloatingActionLightButton#��[  54      $��75��55      $��75��6)    � Cursor$
 Hand��64    J� Effect 56      3  -System.Windows.Media.Effects.DropShadowEffect  57         ShadowDepth$ 20Q�6/      	Direction$	 270Q�6@      Color$ #000000��6P      Opacity$	 0.3Q�58   /      
BlurRadius$ 50Q�6=   .��5:      � .+�5;      �� $	��100��6$   $	��100��60   $��0q�6=   5<      5=      �� 5>      -   5        $��UserLoggedInTextBlock��5@       ���#FFFFFF$��Center=�5>   5   $��Center=�6P   $��
0,20px,0,0q�5?       $��Trebuchet MS)�64   $��15(�6N   Username del Empleado5B      �� 5C      ���#FFFFFF$��Center=�65   $��Center=�6P   $��	0,2px,0,0q�5D       $��Trebuchet MS)�63   $��15(�6M   $	 0.5Q�6[   Puesto5F      5G      �� 5I      
������ $	��0.5��62   $��0,20,0,0q�6?   �� 5K      $��Trebuchet MS)�6   $��15(�66   ���#FFFFFF$
��50px��6Y    �� Padding$ 0,15,0,0q�5L      $��25px,8px,0,0q�6/   $	 0.5Q�6E   MENÚ5N      �� 5P      -   5        $ 
Horizontal=�5P      $
��50px��66   
������ $��	0,-15,0,0q�6]   $
 Hand��5Q        StackPanelHoverStyle#��[ 6+   .+�5R      �� $	��6px��6   $��40��6)   ���#49E12F �� CornerRadius$ 0,3,3,0��6J   $y�Hidden=�6a     5S      $ Solid_Heart=�6'   $��20��6:   $��20��6F   $��25,0,0,0q�6Q    �#FFFFFF$��Center=�5T   ;   $��Center=�6V   $ Usuarios��5U   &   �� 5V      ���#FFFFFF$��Center=�65   $��Center=�6P   $��
18px,0,0,0q�5W       $��Trebuchet MS)�64   $��15(�6N   
Clientes5X      5Y      �� 5[      $ 
Horizontal=�6   $
��50px��66   
������ $
 Hand��6]   #��[ 5\      .+�5]      �� $	��6px��6   $��40��6)   ���#49E12F$ 0,3,3,0��6J   $y�Hidden=�6a     5^      $ 
Solid_User=�6'   $��20��69   $��20��6E   $��25,0,0,0q�6P    �#FFFFFF$��Center=�5_   ;   $��Center=�6V   $ Usuarios��5`   &   �� 5a      ���#FFFFFF$��Center=�65   $��Center=�6P   $��
18px,0,0,0q�5b       $��Trebuchet MS)�64   $��15(�6N   	Empleados5d      5e      �� 5g      $ 
Horizontal=�6   $
��50px��66   
������ $
 Hand��6]   #��[ 5h      .+�5i      �� $	��6px��6   $��40��6)   ���#49E12F$ 0,3,3,0��6J   $y�Hidden=�6a   '  !MaterialDesignThemes.Wpf.PackIcon  5j         Kind$ FoodDrumstick=�6.   ���#FFFFFF$��Center=�6X   $��Center=�5k   .   $��25��6K   $��25��6W   $��21,0,0,0q�6b   �� 5l      ���#FFFFFF$��Center=�65   $��Center=�6P   $��
18px,0,0,0q�5m       $��Trebuchet MS)�64   $��15(�6N   	Productos5o      5p      �� 5r      -   5        $��OrdersStackPanel��5t      $ 
Horizontal=�5r      $
��50px��66   
������ $
 Hand��6]   #��[ 5s      .+�5u      �� -   5        $��OrdersGreendBorder��5v      $	��6px��5u      $��40��6)   ���#49E12F$ 0,3,3,0��6J   $y�Hidden=�6a   5v        5w      $ Solid_ShoppingCart=�6'   $��20��6A   $��20��6M   $��25,0,0,0q�6X    �#FFFFFF$��Center=�5x   ;   $��Center=�6V   $ Usuarios��5y   &   �� 5z      ���#FFFFFF$��Center=�65   $��Center=�6P   $��
18px,0,0,0q�5{       $��Trebuchet MS)�64   $��15(�6N   	Pedidos5}      5~      �� 5�      $ 
Horizontal=�6   $
��50px��66   
������ $
 Hand��6]   #��[ 5�      .+�5�      �� $	��6px��6   $��40��6)   ���#49E12F$ 0,3,3,0��6J   $y�Hidden=�6a     5�      $ ChefHat=�6.   ���#FFFFFF$��Center=�6R   $��Center=�5�   .   $��25��6K   $��25��6W   $��21,0,0,0q�6b   �� 5�      ���#FFFFFF$��Center=�65   $��Center=�6P   $��
18px,0,0,0q�5�       $��Trebuchet MS)�64   $��15(�6N   Cocina5�      5�      �� 5�      $ 
Horizontal=�6   $
��50px��66   
������ $
 Hand��6]   #��[ 5�      .+�5�      �� $	��6px��6   $��40��6)   ���#49E12F$ 0,3,3,0��6J   $y�Hidden=�6a     5�      $ Solid_Coins=�6'   $��20��6:   $��20��6F   $��25,0,0,0q�6Q    �#FFFFFF$��Center=�5�   ;   $��Center=�6V   $ Usuarios��5�   &   �� 5�      ���#FFFFFF$��Center=�65   $��Center=�6P   $��
18px,0,0,0q�5�       $��Trebuchet MS)�64   $��15(�6N   	Finanazas5�      5�      �� 5�      $ 
Horizontal=�6   $
��50px��66   
������ $��0,42,0,0q�6]   $
 Hand��5�      #��[ 6+   $y�Visible=�6Y   .+�5�      �� $	��6px��6   $��40��6)   ���#49E12F$ 0,3,3,0��6J   $y�Hidden=�6a     5�      $ 	ExitToApp=�6.   ���#FFFFFF$��Center=�6T   $��Center=�5�   .   $��25��6K   $��25��6W   $��21,0,0,0q�6b   �� 5�      ���#FFFFFF$��Center=�65   $��Center=�6P   $��
18px,0,0,0q�5�       $��Trebuchet MS)�64   $��15(�6N   Salir5�      5�      5�      5�       � 5�   
   -   5        $��NavigationFrame��5�      $	��776��5�      $	��720��6   $��209.7px,0,0,0q�6)   ���#FFF$
 0.91Q�5�        � NavigationUIVisibility$ Hidden=�66   5�      5�      