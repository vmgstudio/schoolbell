   M S B A M L   `   `   `  ���� F  Bschoolbelll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:schoolbelllschoolbelll      schoolbelll.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:schoolbelll       � Title$  	Csengető��$	��450��$	��800�����	#FF2B2B2B � Icon$ 
csengo.png�� � 
ResizeMode$ NoResize=� � 	Resources %   schoolbelll.MainWindowViewModel   	ViewModel  BoolToVisConverter��%   &	        &	        �� .��� $
��Left=�$	��405��$��	10,10,0,0q�$	��Top=�$	��767�� � ColumnDefinitions �� $��0* ��� .+��� $��Pontos idő:��$
��Left=�$��	240,0,0,0q�$	��Top=�$��2��$��26(�$
��Bold"�
��������� -   $��LiveTimeLabel��$��%TIME%��$
��Left=�$��	393,0,0,0q�$	��Top=�$��45��$	��120��$��26(�$
��Bold"�
�������$��2���� $��2��$��Jelenlegi csengetés:��$
��Left=�$��	10,46,0,0q�$	��Top=�
�������$��20(��� -    ��Name$ selectSchedule��$��2��$
��Left=�$��	10,83,0,0q�$	��Top=�$	��268�����	#FF333333���	#FF333333���	#FF333333$ �� IsSynchronizedWithCurrentItem$ FalseY�$��33��$��Center=����� *ScheduleList+�� -    ��Name$ addSchedule��$��2��$!��Új csengetés hozzáadása��$
��Left=�$��
10,121,0,0q�$	��Top=�$	��207��$��31�����	#FF007E00
�������$
��Bold"�$��2,1,1,1q�&   System.Windows.Controls.DataGrid  -     Name$ datagrid��$��2��$��Center=�$	��340��$��352,55,10,0q�$	��Top=�$	��405��	   AutoGenerateColumns	 .  
   CanUserReorderColumns
 .     CanUserResizeColumns .     RowBackground
 ���� 
������    ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 auto    CanUserAddRows .     CanUserDeleteRows .     CanUserSortColumns .     CanUserResizeRows .  ����  �� Path$ 	Csengetes�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Jelző csengetés��   
Foreground
 �����1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *jelzo+  $ Becsengetés��
 ����� �� *becsengetes+  $ Kicsengetés��
 ����� �� *kicsengetes+�� -   $ 	deleteBtn��$��2��$��Töröl��$
��Left=�$��
286,83,0,0q�$	��Top=�$��46�� J� RenderTransformOrigin$ 0.201,0.454&�$��33��
�������
���  ���� -   $ StopBtn��$��2��$��Szünet��$
��Left=�$��228,121,0,0q�$	��Top=�$	��104��$��31�����	#FFFFC500�� -    ��Name$ 	stopLabel��$��2��$��CSENGETÉS SZÜNETEL!��$��Center=�$
��Left=�$��
10,172,0,0q�$	��Top=�$��42��$	��322��
���  ��$��25(�$
��Bold"�y��� *IsScheduleDisabled+ �� 	Converter# [ �� $��2��$=��7Verzió: 1.0.13. © 2020. Harkai Balázs, Bán Szilárd��$
��Left=�$��
10,369,0,0q�$	��Top=����	#FF686868$	��322��