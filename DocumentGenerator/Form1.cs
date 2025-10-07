using DevExpress.XtraEditors;
using DocumentGenerator.Models;
using DocumentGenerator.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DocumentGenerator
{
    public partial class Form1 : XtraForm
    {
        private IDocumentService _documentService;
        private string _currentTemplate;
        private List<string> _currentTags;

        private SimpleButton btnGenerate;
        public Form1()
        {   
            InitializeComponent();
            InitializeApplication();
        }
        private void InitializeApplication()
        {
            Text = "Document Generator - Генератор документов";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;

            _documentService = new DocumentService();
            _currentTags = new List<string>();

            CreatemainInterface();
        }
        private void CreatemainInterface()
        {
            Panel controlPanel = new Panel()
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.LightGray
            };

            SimpleButton btnLoadTemplate = new SimpleButton()
            {
                Text = "Загрузка шаблона",
                Location = new Point(10, 10),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 10F)
            };
            btnLoadTemplate.Click += BtnLoadTemplate_Click;

            SimpleButton btnExtractTags = new SimpleButton()
            {
                Text = "Загрузка шаблона",
                Location = new Point(170, 10),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 10F)
            };
            btnExtractTags.Click += BtnExtraTags_Click;

            btnGenerate = new SimpleButton()
            {
                Text = "Сгенерировать",
                Location = new Point(330, 10),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 10F),
                Enabled = false
            };
            btnGenerate.Click += BtnGenerate_Click;


            Controls.Add(controlPanel);


            controlPanel.Controls.Add(btnLoadTemplate);
            controlPanel.Controls.Add(btnExtractTags);
            controlPanel.Controls.Add(btnGenerate);

            CreateTemplateEditor();
        }

        private void CreateTemplateEditor()
        {
            // Этот метод мы заполним на следующем этапе
            // Пока просто создадим заглушку
            var label = new Label()
            {
                Text = "Область для редактора шаблонов",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12F, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            Controls.Add(label);
            label.BringToFront();
        }

        private void BtnLoadTemplate_Click(object sender, EventArgs e)
        {
            try
            {
            _currentTemplate = @"
            <h1>Договор оказания услуг</h1>
            <p>Договор №: <b>{contract_number}</b></p>
            <p>Дата: <b>{date}</b></p>
            <p>Клиент: <b>{client_name}</b></p>
            <p>Сумма: <b>{amount}</b> рублей</p>
            <p>Описание услуг: {description}</p>
            <p>Менеджер: {manager}</p>
            <p>Статус: {status}</p>
            <hr>
            <p>Подпись: ________________</p>
            ";

            XtraMessageBox.Show("Текстовый шаблон загружен");

                //UpdateStatys()
            }
            catch (Exception ex) 
            {
                XtraMessageBox.Show($"Ошибка загрузки шаблона: {ex.Message}");
            }
        }
        private void BtnExtraTags_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_currentTemplate))
                {
                    XtraMessageBox.Show("Сначала загрузите шаблон");
                    return;
                }

                _currentTags = _documentService.ExtractTags(_currentTemplate);

                if (_currentTags.Any())
                {
                    string tagInfo = $"Найдено тегов {_currentTags.Count}" + string.Join("\n", _currentTags.Select(t => $"* {t}"));
                    
                    XtraMessageBox.Show(tagInfo);

                    btnGenerate.Enabled = true;
                    //UpdateStatus();
                }
                else
                {
                    XtraMessageBox.Show("Теги не найдены");
                    return;
                }
            }
            catch (Exception ex) 
            {
                XtraMessageBox.Show($"Ошибка извлечения тэгов: {ex.Message}");
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_currentTags.Any())
                {
                    XtraMessageBox.Show("Сначала извлеките теги из шаблона!", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var testData = new List<TagData>
                {
                    new TagData("contract_number", "Д-2024-001"),
                    new TagData("date", DateTime.Now.ToString("dd.MM.yyyy")),
                    new TagData("client_name", "ООО 'Ромашка'"),
                    new TagData("amount", "150 000"),
                    new TagData("description", "Разработка программного обеспечения"),
                    new TagData("manager", "Иванов И.И."),
                    new TagData("status", "активен")
                };

                // Генерируем документ
                var document = _documentService.GenerateDocument(_currentTemplate, testData);

                XtraMessageBox.Show(
                    $"Документ успешно сгенерирован!\n\n" +
                    $"Размер: {document.Size} байт\n" +
                    $"Имя файла: {document.FileName}\n" +
                    $"Тип: {document.ContentType}",
                    "Успех",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                //UpdateStatus($"Документ сгенерирован ({document.Size} байт)");

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Ошибка генерации документа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }


}
