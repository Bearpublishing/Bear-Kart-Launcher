   M S B A M L   `   `   `  ����M  IBear Kart Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Bear_Kart_LauncherBear_Kart_Launcher  5   	   #    Bear_Kart_Launcher.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:Bear_Kart_Launcher     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � Title$ Bear Kart Launcher��5
   	   $	��576��6$   $
��1080��61   .��5      � .+�5   
   �� $��UniformToFill=�6   $��Images/bg.png��6(   �� 5   
   -   5        $	��bk1��5      $	��Top=�6   $��Center=�67   $��50(�6T   
�������$��0,97,0,0q�6u   $��	Bear Kart��6�   �� 5   
   -   5        $	��bk2��5      $	��Top=�6   $��Center=�67   $��50(�6T   
���   �$��	0,234,0,0q�6u   $��Bear Kart 2��6�   �� 5   
   -   5        $	��k3b��5      $	��Top=�6   $��Center=�67   $��50(�6T   
���   �$��	0,366,0,0q�6u   $��Kart 3 Bear��6�   �� 5   
   -   5        $��VersionText��5      $��Bottom=�6'   $��Right=�6B   $��20(�6^   
�������$��
0, 0, 5, 0q�6   �� 5   
   -   5        $��bk2VersionText��5      $��Bottom=�6*   $��Right=�6E   $��20(�6a   
�������$��0,0,5,27q�6�    J� RenderTransformOrigin$ 0.583,-0.285&�6�   �� 5   
   -   5        $��bk1VersionText��5      $��Bottom=�6*   $��Right=�6E   $��20(�6a   
�������$��0,0,5,54q�6�   $ 0.583,-0.285&�6�   �� 5   
   -   5        $��Title��5      $	��Top=�6!   $��Center=�69   $	��100(�6V   
�������$��Bear Kart Launcher��6x   $ 0.498,1.572&�6�   $��	0,-21,0,0q�6�   �� 5   
   -	   5        $��
PlayButton��5      $��Checking for updates...��6#   $��60��6^   $��28(�6j   $
��Bold"�6x   ���	#DDFFFFFF$��	0,438,0,0q�6�   $	��Top=�6�   $��Center=�6�   $	��150��6�   
��� ���$��	10,1,10,1q�6  �� 5   
   -
   5        $��bk2PlayButton��5      $��Checking for updates...��6&   $��60��6d   $��28(�6p   $
��Bold"�6~   ���	#DDFFFFFF$��	0,306,0,0q�6�   $	��Top=�6�   $��Center=�6�   $	��150��6�   
��� ���$��	10,1,10,1q�6  �� 5   
   -   5        $��bk1PlayButton��5      $��Checking for updates...��6&   $��60��6H   $��28(�6T   $
��Bold"�6b   ���	#DDFFFFFF$��	0,169,0,0q�6�   $	��Top=�6�   $��Center=�6�   $	��150��6�   
��� ���$��	10,1,10,1q�6�   6  5      5      