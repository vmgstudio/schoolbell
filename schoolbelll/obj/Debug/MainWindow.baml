   M S B A M L   `   `   `  ����F  Bschoolbelll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:schoolbelllschoolbelll  5   	       schoolbelll.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:schoolbelll     5   	     � Title$  	Csengető��5   	   $	��450��6   $	��800��6'   ���	#FF2B2B2B � Icon$ 
csengo.png��6J    � 
ResizeMode$ NoResize=�6\    � 	Resources 5	      %   schoolbelll.MainWindowViewModel   	ViewModel  BoolToVisConverter��%:   &	        &	        5
   
   6$   �� 5   
   6'   5      .��5      � $
��Left=�6   $	��405��6&   $��	10,10,0,0q�63   $	��Top=�6F   $	��767��6^    � ColumnDefinitions 5   
   �� 5      $��0* �6   �� 5      .+�5   
   �� $��Pontos idő:��6   $
��Left=�6&   $��	240,0,0,0q�6A   $	��Top=�6T   $��2��6l   $��26(�6�   $
��Bold"�6�   
�������6�   �� 5   
   -   5        $��LiveTimeLabel��5      $��%TIME%��6%   $
��Left=�66   $��	393,0,0,0q�6Q   $	��Top=�6d   $��45��6|   $	��120��6�   $��26(�6�   $
��Bold"�6�   
�������$��2��6�   �� 5   
   $��2��6   $��Jelenlegi csengetés:��6$   $
��Left=�6C   $��	10,46,0,0q�6^   $	��Top=�6q   
�������$��20(�6�   �� 5   
   -   5         ��Name$ selectSchedule��5   9   $��2��6Q   $
��Left=�6e   $��	10,83,0,0q�6�   $	��Top=�6�   $	��268��6�   ���	#FF333333���	#FF333333���	#FF333333$ �� IsSynchronizedWithCurrentItem$ FalseY�60  $��33��6V  $��Center=�6b  ��6   �� *ScheduleList+6b  �� 5   
   -   5         ��Name$ addSchedule��5      $��2��6&   $!��Új csengetés hozzáadása��6:   $
��Left=�6\   $��
10,121,0,0q�6w   $	��Top=�6�   $	��207��6�   $��31��6�   ���	#FF007E00
�������$
��Bold"�6�   $��2,1,1,1q�6  &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ datagrid��5      $��2��6%   $��Center=�69   $	��340��6V   $��352,55,10,0q�6c   $	��Top=�6x   $	��405��6�   	   AutoGenerateColumns	 .  
   CanUserReorderColumns
 .     CanUserResizeColumns .     RowBackground
 ���� 
������    ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 auto 6O     CanUserAddRows .     CanUserDeleteRows .     CanUserSortColumns .     CanUserResizeRows .  ��6�   ��  �� Path$ 	Csengetes�   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Jelző csengetés��6%      
Foreground
 �����1  +System.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *jelzo+6X     5      $ Becsengetés��6%   
 ����� 6:   �� *becsengetes+6Z     5      $ Kicsengetés��6%   
 ����� 6:   �� *kicsengetes+6Z   5      �� 5   
   -   5        $ 	deleteBtn��5      $��2��6$   $��Töröl��68   $
��Left=�6H   $��
286,83,0,0q�6c   $	��Top=�6w   $��46��6�    J� RenderTransformOrigin$ 0.201,0.454&�6�   $��33��6�   
�������
���  ��6�   �� 5   
   -   5        $ StopBtn��5      $��2��6"   $��Szünet��66   $
��Left=�6G   $��228,121,0,0q�6b   $	��Top=�6w   $	��104��6�   $��31��6�   ���	#FFFFC5006�   �� 5    
   -   5         ��Name$ 	stopLabel��5       $��2��6}   $��CSENGETÉS SZÜNETEL!��6�   $��Center=�6�   $
��Left=�6�   $��
10,172,0,0q�6�   $	��Top=�6  $��42��6  $	��322��6'  
���  ��$��25(�6D  $
��Bold"�6R  y�6#   �� *IsScheduleDisabled+ �� 	Converter# [ 6R  �� 5!   
   $��2��6   $=��7Verzió: 1.0.12. © 2020. Harkai Balázs, Bán Szilárd��6$   $
��Left=�6a   $��
10,369,0,0q�6|   $	��Top=�6�   ���	#FF686868$	��322��6�   5#      5$      