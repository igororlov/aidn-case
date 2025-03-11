import React, { useState } from 'react';
import { Button, Form, Typography, Alert, InputNumber, ConfigProvider } from 'antd';

const { Title, Text } = Typography;

const themeConfig = {
  token: {
    controlHeight: 40,
    borderRadius: 1,
    fontSize: 16,
  },
};

const App = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [newsScore, setNewsScore] = useState(null);
  const [errorMessage, setErrorMessage] = useState(null);

  const onFinish = async (values) => {
    setLoading(true);
    setNewsScore(null);
    setErrorMessage(null);

    try {
      const response = await fetch("http://localhost:5283/calculate", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          Measurements: [
            { Type: "TEMP", Value: parseFloat(values.temperature) },
            { Type: "HR", Value: parseInt(values.heartrate) },
            { Type: "RR", Value: parseInt(values.respiratoryRate) },
          ],
        }),
      });

      const data = await response.json();
      if (response.ok) {
        setNewsScore(data.score);
      } else {
        setErrorMessage(data || "Error calculating score.");
      }
    } catch (error) {
      setErrorMessage("Failed to connect to the server.");
    }
    setLoading(false);
  };

  return (
    <ConfigProvider theme={themeConfig}>
      <div style={{ maxWidth: 400, margin: "auto", padding: "20px" }}>
        <Title level={3}>NEWS score calculator</Title>
        <Form form={form} layout="vertical" onFinish={onFinish} requiredMark={false}>

          <InputDoubleLabel name="temperature" label="Body temperature" sublabel="Degrees celsius" min={31} max={42}/>
          <InputDoubleLabel name="heartrate" label="Heartrate" sublabel="Beats per minute" min={25} max={220}/>
          <InputDoubleLabel name="respiratoryRate" label="Respiratory rate" sublabel="Breaths per minute" min={3} max={60}/>


          {/* <Form.Item
            label={<div><Text strong>Body temperature</Text><br/><Text style={{fontSize: '14px'}}>Degrees celsius</Text></div>}
            name="temperature"
            rules={[
              { required: true, message: "Enter temperature" },
              { pattern: /^[0-9]+(\.[0-9]+)?$/, message: "Enter a valid number" },
            ]}
          >
            <InputNumber controls={false} style={{
                width: '100%',
              }} placeholder="Degrees Celsius" min={31} max={42}/>
          </Form.Item> */}

          {/* <Form.Item
            label="Heartrate"
            name="heartrate"
            rules={[
              { required: true, message: "Enter heart rate" },
              { pattern: /^[0-9]+$/, message: "Enter a valid number" },
            ]}
          >
            <Input placeholder="Beats per minute" />
          </Form.Item> */}

          {/* <Form.Item
            label="Respiratory rate"
            name="respiratoryRate"
            rules={[
              { required: true, message: "Enter respiratory rate" },
              { pattern: /^[0-9]+$/, message: "Enter a valid number" },
            ]}
          >
            <Input placeholder="Breaths per minute" />
          </Form.Item> */}


          <Form.Item>
            <Button type="primary" htmlType="submit" loading={loading} style={{ borderRadius: 12, marginRight: 10, backgroundColor: "#7424DA" }}>
              Calculate NEWS score
            </Button>

            <Button style={{ borderRadius: 12}} onClick={() => {
              form.resetFields();
              setNewsScore(null);
              setErrorMessage(null);
              }}>Reset form</Button>
          </Form.Item>

        </Form>

        {newsScore &&
          <Alert style={{ borderRadius: 10 }}
            message={<>News score: <strong>{newsScore}</strong></>} type="info" />
        }

        {errorMessage && (
          <Alert style={{ borderRadius: 10 }}
            message={errorMessage} type="error" />
        )}

      </div>
    </ConfigProvider>
  );
};

const InputDoubleLabel = ({ name, label, sublabel, min, max }) => (
    <Form.Item
      label={<div><Text strong>{label}</Text><br/><Text style={{fontSize: '14px'}}>{sublabel}</Text></div>}
      name={name}
      rules={[
        { required: true, message: "Required field" },
        { pattern: /^[0-9]+(\.[0-9]+)?$/, message: "Enter a valid number" },
      ]}
    >
      <InputNumber controls={false} style={{
          width: '100%',
        }} placeholder={sublabel} min={min} max={max}/>
    </Form.Item>
);


export default App;